using System.Collections.Generic;
using Enemy;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        public List<ISavedProgressReader> ProgressReaders { get; }
        public List<ISavedProgress> ProgressWriters { get; }

        public Player.Player CreatePlayer();

        public SpawnManager CreateSpawnManager();

        public void CreateHud(SpawnManager spawnManager, string sceneName, Player.Player player);

        public GameObject CreateBullet();

        public void CleanUp();
    }
}
