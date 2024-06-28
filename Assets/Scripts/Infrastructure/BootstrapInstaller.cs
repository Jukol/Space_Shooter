using Ammo;
using EnemyScripts;
using Logic;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain curtain;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BulletContainer bulletParent;
        [SerializeField] private Camera shakingCamera;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private string initialLevel;
        [SerializeField] private int initialPlayerHealth;
        [SerializeField] private int initialEnemyHealth;

        public override void InstallBindings()
        {
            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(curtain).AsSingle();
            Container.Bind<SpriteRenderer>().FromComponentInNewPrefab(spriteRenderer).AsSingle();
            Container.Bind<BulletContainer>().FromComponentInNewPrefab(bulletParent).AsSingle();
            Container.Bind<Camera>().FromComponentInNewPrefab(shakingCamera).AsSingle();
            Container.Bind<SpawnManager>().FromComponentInNewPrefab(spawnManager).AsSingle();
            Container.BindInstance(initialLevel);
            Container.BindInstance(initialPlayerHealth).WithId("PlayerHealth");;
            Container.BindInstance(initialEnemyHealth).WithId("EnemyHealth");;
        }
    }
}
