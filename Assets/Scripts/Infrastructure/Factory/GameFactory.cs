using System.Collections.Generic;
using Enemy;
using HUD;
using Infrastructure.AssetManagement;
using Infrastructure.Services.PersistentProgress;
using MyScreen;
using Player;
using UnityEngine;
namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private readonly CameraShake _cameraShake;

        public GameFactory(IAssets assets, CameraShake cameraShake)
        {
            _cameraShake = cameraShake;
            _assets = assets;
        }

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();

        public Player.Player CreatePlayer()
        {
            return InstantiateRegisteredPlayer(AssetPaths.PlayerPath);
        }

        public SpawnManager CreateSpawnManager()
        {
            return InstantiateRegisteredSpawnManager(AssetPaths.SpawnManagerPath);
        }

        public void CreateHud(SpawnManager spawnManager, string sceneName, Player.Player player)
        {
            GameObject hudGo = _assets.Instantiate(AssetPaths.HudPath);
            HudData hud = hudGo.GetComponent<HudData>();
            hud.Init(spawnManager, sceneName, player);
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

        private Player.Player InstantiateRegisteredPlayer(string prefabPath)
        {
            Player.Player player = _assets.Instantiate(prefabPath).GetComponent<Player.Player>();
            Register(player);
            player.Init(_cameraShake);
            return player;
        }

        private SpawnManager InstantiateRegisteredSpawnManager(string prefabPath)
        {
            SpawnManager spawnManager = _assets.Instantiate(prefabPath).GetComponent<SpawnManager>();
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
