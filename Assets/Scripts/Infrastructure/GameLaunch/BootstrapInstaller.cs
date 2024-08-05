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
using PlayerScripts;
using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure.GameLaunch
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain curtain;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BulletContainer bulletParent;
        [SerializeField] private Camera myCamera;
        [SerializeField] private SpawnManagerHolder spawnManagerHolder;
        [SerializeField] private string initialLevel;
        [SerializeField] private int initialWave;
        [SerializeField] private int initialSpawnManagerIndex;
        [SerializeField] private int initialPlayerHealth;
        [SerializeField] private int initialEnemyHealth;
        [SerializeField] private int playerShip;
        [SerializeField] private int initialPlayerUpgradeLevel;
        [SerializeField] private StartMenuController startMenuController;
        [SerializeField] private PlayerUpgradeDataList playerUpgradeDataList;
        

        public override void InstallBindings()
        {
            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(curtain).AsSingle();
            Container.Bind<StartMenuController>().FromComponentInNewPrefab(startMenuController).AsSingle();
            Container.Bind<PlayerUpgradeDataList>().FromInstance(playerUpgradeDataList).AsSingle();
            Container.BindInstance(initialLevel);
            Container.BindInstance(initialPlayerHealth).WithId("PlayerHealth");
            Container.BindInstance(playerShip).WithId("PlayerShip");
            Container.BindInstance(initialPlayerUpgradeLevel).WithId("InitialPlayerUpgradeLevel");
            Container.Bind<SpawnManagerHolder>().FromComponentInNewPrefab(spawnManagerHolder).AsSingle();
            Container.Bind<SpawnWrapperHolder>().AsSingle();
            Container.BindInstance(initialWave).WithId("InitialWave");
            Container.BindInstance(initialSpawnManagerIndex).WithId("SpawnManagerIndex");
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<GameInitializer>().AsSingle();
            
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<BootstrapLoaded>();
            Container.DeclareSignal<ProgressLoaded>();
            Container.DeclareSignal<LevelLoadLoaded>().OptionalSubscriber();
            Container.DeclareSignal<LevelCompleted>().OptionalSubscriber();
            Container.DeclareSignal<WaveCompleted>();

            Container.Bind<SpriteRenderer>().FromComponentInNewPrefab(spriteRenderer).AsSingle();
            Container.Bind<BulletContainer>().FromComponentInNewPrefab(bulletParent).AsSingle();
            Container.Bind<Camera>().FromComponentInNewPrefab(myCamera).AsSingle();
            Container.BindInstance(initialEnemyHealth).WithId("EnemyHealth");

            Container.Bind<IAssets>().To<AssetProvider>().AsSingle();
            Container.Bind<CurrentScreen>().AsSingle();
            Container.Bind<IBackgroundAdjuster>().To<BackgroundAdjuster>().AsSingle();

            Container.Bind<IPool>().To<BulletPool>().AsSingle();
            
            Container.Bind<IInput>().To<MouseInput>().AsSingle();
        }
    }
}
