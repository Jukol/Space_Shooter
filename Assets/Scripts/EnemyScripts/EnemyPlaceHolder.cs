using System;
using Infrastructure.Services;
using Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace EnemyScripts
{
    [Serializable]
    public class EnemyPlaceHolder : MonoBehaviour
    {
        public Vector2 Position { get; set; }
        public EnemyStatus enemyStatus;
        private ISaveLoadService _saveLoadService;

        private void Awake()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        public void UpdateStatus(int health)
        {
            enemyStatus.health = health;
            if (enemyStatus.health == 0)
            {
                enemyStatus.dead = true;
            }
            
            _saveLoadService.SaveProgress();
        }
    }
}
