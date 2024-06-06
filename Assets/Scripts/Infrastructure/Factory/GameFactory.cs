using System.Collections.Generic;
using EnemyScripts;
using HUD;
using Infrastructure.AssetManagement;
using Infrastructure.Services.PersistentProgress;
using MyScreen;
using PlayerScripts;
using UnityEngine;
namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly CameraShake _cameraShake;
        private readonly IPersistentProgressService _progressService;

        public GameFactory(IAssets assets, CameraShake cameraShake, IPersistentProgressService progressService)
        {
            _cameraShake = cameraShake;
            _assets = assets;
            _progressService = progressService;
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

        public void CreateHud(SpawnManager spawnManager, string sceneName, Player player)
        {
            GameObject hudGo = _assets.Instantiate(AssetPaths.HudPath);
            HudData hud = hudGo.GetComponent<HudData>();

            int waveNumber = _progressService.Progress.lastState.waveToLoad;
            
            hud.Init(spawnManager, sceneName, player, waveNumber);
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

        private Player InstantiateRegisteredPlayer(string prefabPath)
        {
            Player player = _assets.Instantiate(prefabPath).GetComponent<Player>();
            Register(player);
            player.Init(_cameraShake);
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
            SpawnManager spawnManager = _assets.Instantiate(prefabPath).GetComponent<SpawnManager>();
            spawnManager.Init(this, _progressService);
            Register(spawnManager);
            return spawnManager;
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
