using System.Collections.Generic;
using System.Runtime.InteropServices;
using DefaultNamespace;
using EnemyScripts;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.States;
using Logic;
using MyScreen;
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
        private readonly int _wave = 0;

        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;
        
        [Inject]
        public void Construct(
            IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService,
            IGameFactory gameFactory)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _gameFactory = gameFactory;
        }

        private void Awake()
        {
            spawnersWrapper = CreateWrapperOfListOfSpawners();

            _game = new Game(this, 
                curtain,
                initialLevel,
                initialPlayerHealth,
                initialEnemyHealth, 
                spawnersWrapper,
                _wave,
                _progressService,
                _saveLoadService,
                _gameFactory);
            
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }

        private SpawnersWrapper CreateWrapperOfListOfSpawners()
        {
            SpawnersWrapper spawnersWrapper = new ();
            spawnersWrapper.WrapperOfStatuses = new List<StatusesWrapper>();

            for (int i = 0; i < spawnManager.spawners.Length; i++)
            {
                StatusesWrapper statusesWrapper = new ();
                statusesWrapper.ListOfStatuses = new List<EnemyStatus>();

                for (int j = 0; j < spawnManager.spawners[i].enemyPlaceHolders.Length; j++)
                {
                    EnemyStatus enemyStatus = new(i, j, initialEnemyHealth, false);
                    statusesWrapper.ListOfStatuses.Add(enemyStatus);
                }
                
                spawnersWrapper.WrapperOfStatuses.Add(statusesWrapper);
            }

            return spawnersWrapper;
        }
    }
}
