using System.Collections.Generic;
using EnemyScripts;
using PlayerScripts;
using UnityEngine;

namespace Interfaces
{
    public interface IGameFactory : IService
    {
        public List<ISavedProgressReader> ProgressReaders { get; }
        public List<ISavedProgressWriter> ProgressWriters { get; }

        public Player CreatePlayer();

        public Enemy CreateEnemy(Transform gridStartPosition, EnemyPlaceHolder placeHolder);

        public SpawnManager CreateSpawnManager();
        public void LaunchSpawnManager();

        public void CreateHud(SpawnManager spawnManager, string sceneName, Player player, IPersistentProgressService progress);

        public GameObject CreateBullet();

        public void CleanUp();
    }
}
