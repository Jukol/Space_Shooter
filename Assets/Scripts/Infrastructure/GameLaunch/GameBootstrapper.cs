using EnemyScripts;
using Infrastructure.Signals;
using Infrastructure.States;
using Interfaces;
using PlayerScripts;
using UnityEngine;
using Zenject;

namespace Infrastructure.GameLaunch
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private GameInitializer _gameInitializer;
        private Game _game;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(GameInitializer gameInitializer, SignalBus signalBus)
        {
            _gameInitializer = gameInitializer;
            _signalBus = signalBus;
            
            _signalBus.Subscribe<BootstrapLoaded>(OnBootstrapStateLoaded);
            _signalBus.Subscribe<ProgressLoaded>(OnProgressStateLoaded);
            _signalBus.Subscribe<LevelLoadLoaded>(OnLoadLevelStateLoaded);
        }

        private void Awake()
        {
            _game = new Game(_gameInitializer, this, _signalBus);
            
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }

        private void OnBootstrapStateLoaded()
        {
            _game.StateMachine.Enter<LoadProgressState>();
        }

        private void OnProgressStateLoaded()
        {
            _gameInitializer.GameFactory.CleanUp();
            _game.StateMachine.Enter<LoadLevelState, string>(_gameInitializer.ProgressService.Progress.lastState.levelToLoad);
        }

        private void OnLoadLevelStateLoaded()
        {
            InitGameWorld(_gameInitializer.ProgressService.Progress.lastState.levelToLoad);
            InformProgressReaders();
            
            _gameInitializer.GameFactory.LaunchSpawnManager();

            _game.StateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld(string sceneName)
        {
            Player player = _gameInitializer.GameFactory.CreatePlayer();
            SpawnManager spawnManager = _gameInitializer.GameFactory.CreateSpawnManager();
            _gameInitializer.GameFactory.CreateHud(
                spawnManager, 
                sceneName, 
                player, 
                _gameInitializer.ProgressService);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameInitializer.GameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_gameInitializer.ProgressService.Progress);
            }
        }
    }
}
