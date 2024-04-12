using Ammo;
using Background;
using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using InputClasses;
using MyScreen;
using UnityEngine;
namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Boot = "Boot";
        private readonly BulletContainer _bulletContainer;
        private readonly Camera _camera;
        private readonly CameraShake _cameraShake;
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly SpriteRenderer _spriteRenderer;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services,
            Camera camera, SpriteRenderer spriteRenderer, BulletContainer bulletContainer, CameraShake cameraShake)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _camera = camera;
            _spriteRenderer = spriteRenderer;
            _bulletContainer = bulletContainer;
            _cameraShake = cameraShake;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Boot, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {

        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IBackgroundAdjuster>(new BackgroundAdjuster(_camera, _spriteRenderer));
            _services.RegisterSingle<IAssets>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>(), _cameraShake));
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
            _services.RegisterSingle<IPool>(new BulletPool(_services.Single<IGameFactory>(), _bulletContainer));
            _services.RegisterSingle(new CurrentScreen(_camera));
            _services.RegisterSingle<IInput>(new MouseInput());
        }
    }
}
