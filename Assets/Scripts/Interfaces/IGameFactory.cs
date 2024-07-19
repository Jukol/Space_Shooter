using System.Collections.Generic;
using EnemyScripts;
using Infrastructure.GameLaunch;
using PlayerScripts;
using UnityEngine;
using Zenject;

namespace Interfaces
{
    public interface IGameFactory : IService
    {
        public List<ISavedProgressReader> ProgressReaders { get; }
        public List<ISavedProgressWriter> ProgressWriters { get; }

        public Player CreatePlayer();

        public Enemy CreateEnemy(Transform gridStartPosition, EnemyPlaceHolder placeHolder);

        public void CreateHud(string sceneName, Player player, IPersistentProgressService progress);
        public SpawnWrapperHolder CreateSpawnWrapperHolder();
        
        public SpawnManager CreateSpawnManager(string scene);

        public GameObject CreateBullet();

        public void CleanUp();
    }
}
