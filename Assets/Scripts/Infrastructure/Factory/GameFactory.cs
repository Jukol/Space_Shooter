using System.Collections.Generic;
using EnemyScripts;
using HUD;
using Infrastructure.AssetManagement;
using Interfaces;
using MyScreen;
using PlayerScripts;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private SpawnManager SpawnManager { get; set; }
        
        private readonly IAssets _assets;
        private readonly Camera _camera;
        private readonly IPersistentProgressService _progressService;
        private readonly CurrentScreen _currentScreen;
        private readonly SignalBus _signalBus;
        
        public GameFactory(
            IAssets assets, 
            Camera camera, 
            IPersistentProgressService progressService,
            CurrentScreen currentScreen,
            SignalBus signalBus)
        {
            _camera = camera;
            _assets = assets;
            _progressService = progressService;
            _currentScreen = currentScreen;
            _signalBus = signalBus;
        }

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgressWriter> ProgressWriters { get; } = new();

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

        public void CreateHud(
            string sceneName, 
            Player player, 
            IPersistentProgressService progressService)
        {
            InstantiateRegisteredHud(_signalBus, sceneName, player, progressService);
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

        private void InstantiateRegisteredHud(
            SignalBus signalBus, 
            string sceneName, 
            Player player, 
            IPersistentProgressService progressService)
        {
            GameObject hudGo = _assets.Instantiate(AssetPaths.HudPath);
            HudData hud = hudGo.GetComponent<HudData>();

            int waveNumber = _progressService.Progress.lastState.waveToLoad;
            int killCount = progressService.Progress.lastState.killCount;
            
            hud.Init(signalBus, sceneName, player, waveNumber, killCount);
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
            if (progressReader is ISavedProgressWriter progressWriter)
            {
                ProgressWriters.Add(progressWriter);
            }

            ProgressReaders.Add(progressReader);
        }
    }
}
