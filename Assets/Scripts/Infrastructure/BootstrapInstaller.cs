using Ammo;
using Background;
using EnemyScripts;
using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using InputClasses;
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
        [SerializeField] private CameraShake shakingCamera;
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
            Container.Bind<CameraShake>().FromComponentInNewPrefab(shakingCamera).AsSingle();
            Container.Bind<SpawnManager>().FromComponentInNewPrefab(spawnManager).AsSingle();
            Container.BindInstance(initialLevel);
            Container.BindInstance(initialPlayerHealth).WithId("PlayerHealth");
            Container.BindInstance(initialEnemyHealth).WithId("EnemyHealth");


            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IAssets>().To<AssetProvider>().AsSingle();
            
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();

            Container.Bind<IPool>().To<BulletPool>().AsSingle();
            
            Container.Bind<IBackgroundAdjuster>().To<BackgroundAdjuster>().AsSingle();
            Container.Bind<CurrentScreen>().AsSingle();

            Container.Bind<IInput>().To<MouseInput>().AsSingle();
        }
    }
}
