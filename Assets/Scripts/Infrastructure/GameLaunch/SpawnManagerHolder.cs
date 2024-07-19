using System;
using System.Collections.Generic;
using EnemyScripts;
using UnityEngine;
using Zenject;

namespace Infrastructure.GameLaunch
{
    public class SpawnManagerHolder : MonoBehaviour
    {
        [SerializeField] private SpawnManager[] spawnManagers;

        public SpawnManager[] SpawnManagers => spawnManagers;

        public Dictionary<string, SpawnManager> SpawnManagersByScene;

        [Inject]
        private void Construct()
        {
            SpawnManagersByScene = new()
            {
                { "Level 1", spawnManagers[0] },
                { "Level 2", spawnManagers[1] },
            };
        }
    }
}