using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int initialHealth;
        [SerializeField] private SpawnManager spawnManager;


        private CameraShake cameraShake;

        private Game _game;

        private WrapperOfListOfSpawners wrapperOfListOfSpawners;

        private void Awake()
        {
            LoadingCurtain loadingCurtain = Instantiate(curtain);
            SpriteRenderer sprRenderer = Instantiate(spriteRenderer);
            BulletContainer bulletContainer = Instantiate(bulletParent);
            Camera shkCamera = Instantiate(shakingCamera);

            cameraShake = shkCamera.GetComponent<CameraShake>();

            wrapperOfListOfSpawners = CreateWrapperOfListOfSpawners();

            _game = new Game(this, 
                loadingCurtain,
                shkCamera, 
                sprRenderer, 
                bulletContainer, 
                cameraShake,
                initialLevel,
                initialHealth, 
                wrapperOfListOfSpawners);
            
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }

        private WrapperOfListOfSpawners CreateWrapperOfListOfSpawners()
        {
            WrapperOfListOfSpawners allEnemyStatuses = new ();
            allEnemyStatuses.WrapperOfStatuses = new List<WrapperOfListOfStatuses>();

            for (int i = 0; i < spawnManager.spawners.Length; i++)
            {
                WrapperOfListOfStatuses wrapper = new ();
                wrapper.ListOfStatuses = new List<EnemyStatus>();

                for (int j = 0; j < spawnManager.spawners[i].enemyPlaceHolders.Length; j++)
                {
                    EnemyStatus enemyStatus = new(i, j, 50, false);
                    wrapper.ListOfStatuses.Add(enemyStatus);
                }
                
                allEnemyStatuses.WrapperOfStatuses.Add(wrapper);
            }

            return allEnemyStatuses;
        }
    }
}
