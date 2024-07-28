using System.Collections.Generic;
using EnemyScripts;
using Infrastructure.GameLaunch;
using PlayerScripts;
using UI;
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

        public void CreateHud(Player player, IPersistentProgressService progress, SpawnManager spawnManager);
        
        public StartMenuController CreateStartMenu();
        public SpawnWrapperHolder CreateSpawnWrapperHolder();
        
        public SpawnManager CreateSpawnManager(string scene);

        public GameObject CreateBullet();

        public void CleanUp();
    }
}
