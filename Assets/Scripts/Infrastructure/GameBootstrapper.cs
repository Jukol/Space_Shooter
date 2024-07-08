using System.Collections.Generic;
using EnemyScripts;
using Infrastructure.Signals;
using Infrastructure.States;
using Interfaces;
using Logic;
using PlayerScripts;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [Inject] private LoadingCurtain curtain;
        [Inject] private string initialLevel;
        [Inject (Id = "PlayerHealth")] private int initialPlayerHealth;
        [Inject (Id = "EnemyHealth")] private int initialEnemyHealth;
        [Inject] private SpawnManager spawnManager;

        private Game _game;
        private SpawnersWrapper spawnersWrapper;
        private int _wave;

        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;
        
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(
            IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService,
            IGameFactory gameFactory,
            SignalBus signalBus)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _gameFactory = gameFactory;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            spawnersWrapper = CreateWrapperOfListOfSpawners();
            _signalBus.Subscribe<BootstrapLoaded>(OnBootstrapStateLoaded);
            _signalBus.Subscribe<ProgressLoaded>(OnProgressStateLoaded);
            _signalBus.Subscribe<LevelLoadLoaded>(OnLoadLevelStateLoaded);

            _game = new Game(this, 
                curtain,
                initialLevel,
                initialPlayerHealth,
                spawnersWrapper,
                _wave,
                _progressService,
                _saveLoadService,
                _gameFactory,
                _signalBus);
            
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }

        private void OnBootstrapStateLoaded() => _game.StateMachine.Enter<LoadProgressState>();
        private void OnProgressStateLoaded()
        {
            _gameFactory.CleanUp();
            _game.StateMachine.Enter<LoadLevelState, string>(_progressService.Progress.lastState.levelToLoad);
        }

        private void OnLoadLevelStateLoaded()
        {
            InitGameWorld(_progressService.Progress.lastState.levelToLoad);
            InformProgressReaders();
            
            _gameFactory.LaunchSpawnManager();

            _game.StateMachine.Enter<GameLoopState>();
        }

        private SpawnersWrapper CreateWrapperOfListOfSpawners()
        {
            SpawnersWrapper mySpawnersWrapper = new ();
            mySpawnersWrapper.WrapperOfStatuses = new List<StatusesWrapper>();

            for (int i = 0; i < spawnManager.spawners.Length; i++)
            {
                StatusesWrapper statusesWrapper = new ();
                statusesWrapper.ListOfStatuses = new List<EnemyStatus>();

                for (int j = 0; j < spawnManager.spawners[i].enemyPlaceHolders.Length; j++)
                {
                    EnemyStatus enemyStatus = new(initialEnemyHealth, false);
                    statusesWrapper.ListOfStatuses.Add(enemyStatus);
                }
                
                mySpawnersWrapper.WrapperOfStatuses.Add(statusesWrapper);
            }

            return mySpawnersWrapper;
        }
        
        private void InitGameWorld(string sceneName)
        {
            Player player = _gameFactory.CreatePlayer();
            SpawnManager spawnManager = _gameFactory.CreateSpawnManager();
            _gameFactory.CreateHud(spawnManager, sceneName, player, _progressService);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }
    }
}
