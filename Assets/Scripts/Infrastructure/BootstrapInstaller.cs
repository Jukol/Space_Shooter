using System.Collections.Generic;
using Ammo;
using Background;
using EnemyScripts;
using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Signals;
using InputClasses;
using Interfaces;
using Logic;
using MyScreen;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain curtain;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BulletContainer bulletParent;
        [SerializeField] private Camera myCamera;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private string initialLevel;
        [SerializeField] private int initialWave;
        [SerializeField] private int initialPlayerHealth;
        [SerializeField] private int initialEnemyHealth;

        public override void InstallBindings()
        {
            //Game Initializer
            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(curtain).AsSingle();
            Container.BindInstance(initialLevel);
            Container.BindInstance(initialPlayerHealth).WithId("PlayerHealth");
            Container.Bind<SpawnersWrapper>().FromMethod(CreateWrapperOfListOfSpawners).AsSingle();
            Container.BindInstance(initialWave).WithId("InitialWave");
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<GameInitializer>().AsSingle();
            
            //Signals
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<BootstrapLoaded>();
            Container.DeclareSignal<ProgressLoaded>();
            Container.DeclareSignal<LevelLoadLoaded>();

            Container.Bind<SpriteRenderer>().FromComponentInNewPrefab(spriteRenderer).AsSingle();
            Container.Bind<BulletContainer>().FromComponentInNewPrefab(bulletParent).AsSingle();
            Container.Bind<Camera>().FromComponentInNewPrefab(myCamera).AsSingle();
            Container.Bind<SpawnManager>().FromComponentInNewPrefab(spawnManager).AsSingle();
            Container.BindInstance(initialEnemyHealth).WithId("EnemyHealth");

            Container.Bind<IAssets>().To<AssetProvider>().AsSingle();
            Container.Bind<CurrentScreen>().AsSingle();
            Container.Bind<IBackgroundAdjuster>().To<BackgroundAdjuster>().AsSingle();

            Container.Bind<IPool>().To<BulletPool>().AsSingle();
            
            Container.Bind<IInput>().To<MouseInput>().AsSingle();
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
    }
}
