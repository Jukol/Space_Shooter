using System.Collections.Generic;
using Ammo;
using DefaultNamespace;
using EnemyScripts;
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
        [Inject] private SpriteRenderer spriteRenderer;
        [Inject] private BulletContainer bulletParent;
        [Inject] private Camera shakingCamera;
        [Inject] private string initialLevel;
        [Inject (Id = "PlayerHealth")] private int initialPlayerHealth;
        [Inject (Id = "EnemyHealth")] private int initialEnemyHealth;
        [Inject] private SpawnManager spawnManager;

        private CameraShake cameraShake;

        private Game _game;

        private SpawnersWrapper spawnersWrapper;

        private readonly int _wave = 0;

        private void Awake()
        {
            cameraShake = shakingCamera.GetComponent<CameraShake>();
            spawnersWrapper = CreateWrapperOfListOfSpawners();

            _game = new Game(this, 
                curtain,
                shakingCamera, 
                spriteRenderer, 
                bulletParent, 
                cameraShake,
                initialLevel,
                initialPlayerHealth,
                initialEnemyHealth, 
                spawnersWrapper,
                _wave);
            
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
