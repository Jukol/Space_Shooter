using Ammo;
using Background;
using EnemyScripts;
using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
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
        [SerializeField] private int initialPlayerHealth;
        [SerializeField] private int initialEnemyHealth;

        public override void InstallBindings()
        {
            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(curtain).AsSingle();
            Container.Bind<SpriteRenderer>().FromComponentInNewPrefab(spriteRenderer).AsSingle();
            Container.Bind<BulletContainer>().FromComponentInNewPrefab(bulletParent).AsSingle();
            Container.Bind<Camera>().FromComponentInNewPrefab(myCamera).AsSingle();
            Container.Bind<SpawnManager>().FromComponentInNewPrefab(spawnManager).AsSingle();
            Container.BindInstance(initialLevel);
            Container.BindInstance(initialPlayerHealth).WithId("PlayerHealth");
            Container.BindInstance(initialEnemyHealth).WithId("EnemyHealth");

            Container.Bind<IAssets>().To<AssetProvider>().AsSingle();
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<CurrentScreen>().AsSingle();
            Container.Bind<IBackgroundAdjuster>().To<BackgroundAdjuster>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();

            Container.Bind<IPool>().To<BulletPool>().AsSingle();

            Container.Bind<IInput>().To<MouseInput>().AsSingle();
        }
    }
}
