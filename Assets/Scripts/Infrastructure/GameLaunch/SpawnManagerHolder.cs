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

        public Dictionary<string, SpawnManager> SpawnManagersByScene = new();

        [Inject]
        private void Construct()
        {
            Enumerate();
            
            for (int i = 0; i < spawnManagers.Length; i++)
            {
                string sceneName = $"Level {i + 1}";

                SpawnManagersByScene.Add(sceneName, spawnManagers[i]);
            }
            

        }

        private void Enumerate()
        {
            for (var i = 0; i < spawnManagers.Length; i++)
            {
                spawnManagers[i].id = i;
            }
        }
    }
}