using System.Collections.Generic;
using EnemyScripts;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using PlayerScripts;
using UnityEngine;
namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        public List<ISavedProgressReader> ProgressReaders { get; }
        public List<ISavedProgress> ProgressWriters { get; }

        public Player CreatePlayer();

        public Enemy CreateEnemy(Transform gridStartPosition, EnemyPlaceHolder placeHolder);

        public SpawnManager CreateSpawnManager();
        public void LaunchSpawnManager();

        public void CreateHud(SpawnManager spawnManager, string sceneName, Player player, IPersistentProgressService progress);

        public GameObject CreateBullet();

        public void CleanUp();
    }
}
