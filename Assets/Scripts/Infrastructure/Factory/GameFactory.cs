using System.Collections.Generic;
using Background;
using EnemyScripts;
using HUD;
using Infrastructure.AssetManagement;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using MyScreen;
using PlayerScripts;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        public SpawnManager SpawnManager { get; private set; }
        
        private readonly IAssets _assets;
        private readonly Camera _camera;
        private readonly IPersistentProgressService _progressService;
        private readonly CurrentScreen _currentScreen;

        [Inject]
        public GameFactory(
            IAssets assets, 
            Camera camera, 
            IPersistentProgressService progressService,
            CurrentScreen currentScreen,
            IBackgroundAdjuster adjuster)
        {
            _camera = camera;
            _assets = assets;
            _progressService = progressService;
            _currentScreen = currentScreen;
        }

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();

        public Player CreatePlayer()
        {
            return InstantiateRegisteredPlayer(AssetPaths.PlayerPath);
        }

        public Enemy CreateEnemy(Transform gridStartPosition, EnemyPlaceHolder placeHolder)
        {
            return InstantiateEnemy(AssetPaths.EnemyPath, gridStartPosition, placeHolder);
        }

        public SpawnManager CreateSpawnManager()
        {
            return InstantiateRegisteredSpawnManager(AssetPaths.SpawnManagerPath);
        }

        public void CreateHud(SpawnManager spawnManager, string sceneName, Player player, IPersistentProgressService progressService)
        {
            InstantiateRegisteredHud(spawnManager, sceneName, player, progressService);
        }

        public GameObject CreateBullet()
        {
            return _assets.Instantiate(AssetPaths.BulletPath);
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
        
        public void LaunchSpawnManager()
        {
            SpawnManager.Launch();
        }

        private void InstantiateRegisteredHud(SpawnManager spawnManager, string sceneName, Player player, IPersistentProgressService progressService)
        {
            GameObject hudGo = _assets.Instantiate(AssetPaths.HudPath);
            HudData hud = hudGo.GetComponent<HudData>();

            int waveNumber = _progressService.Progress.lastState.waveToLoad;
            int killCount = progressService.Progress.lastState.killCount;
            
            hud.Init(spawnManager, sceneName, player, waveNumber, killCount);
            Register(hud);
        }

        private Player InstantiateRegisteredPlayer(string prefabPath)
        {
            Player player = _assets.Instantiate(prefabPath).GetComponent<Player>();
            Register(player);
            var cameraShake = _camera.GetComponent<CameraShake>();
            player.Init(cameraShake, _currentScreen);
            return player;
        }

        private Enemy InstantiateEnemy(string prefabPath, Transform gridStartPosition, EnemyPlaceHolder placeHolder)
        {
            Enemy enemy = _assets.Instantiate(prefabPath, gridStartPosition, Quaternion.Euler(0, 0, 180)).GetComponent<Enemy>();
            enemy.transform.SetParent(placeHolder.transform, true);
            enemy.Init(placeHolder.enemyStatus.health);

            return enemy;
        }

        private SpawnManager InstantiateRegisteredSpawnManager(string prefabPath)
        {
            SpawnManager = _assets.Instantiate(prefabPath).GetComponent<SpawnManager>();
            Register(SpawnManager);
            return SpawnManager;
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                ProgressWriters.Add(progressWriter);
            }

            ProgressReaders.Add(progressReader);
        }
    }
}
