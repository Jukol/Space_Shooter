using System.Collections.Generic;
using Ammo;
using DefaultNamespace;
using EnemyScripts;
using Infrastructure.States;
using Logic;
using MyScreen;
using UnityEngine;
namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain curtain;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BulletContainer bulletParent;
        [SerializeField] private Camera shakingCamera;
        [SerializeField] private string initialLevel;
        [SerializeField] private int initialPlayerHealth;
        [SerializeField] private int initialEnemyHealth;
        [SerializeField] private SpawnManager spawnManager;

        private CameraShake cameraShake;

        private Game _game;

        private SpawnersWrapper spawnersWrapper;

        private readonly int _wave = 0;

        private void Awake()
        {
            LoadingCurtain loadingCurtain = Instantiate(curtain);
            SpriteRenderer sprRenderer = Instantiate(spriteRenderer);
            BulletContainer bulletContainer = Instantiate(bulletParent);
            Camera shkCamera = Instantiate(shakingCamera);

            cameraShake = shkCamera.GetComponent<CameraShake>();

            spawnersWrapper = CreateWrapperOfListOfSpawners();

            _game = new Game(this, 
                loadingCurtain,
                shkCamera, 
                sprRenderer, 
                bulletContainer, 
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
