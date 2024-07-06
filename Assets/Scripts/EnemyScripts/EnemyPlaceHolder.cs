using System;
using Infrastructure.Services;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace EnemyScripts
{
    [Serializable]
    public class EnemyPlaceHolder : MonoBehaviour
    {
        public event Action Dead;
        
        public Vector2 Position { get; set; }
        public EnemyStatus enemyStatus;

        public void UpdateStatus(int health, ISaveLoadService saveLoadService)
        {
            enemyStatus.health = health;
            if (enemyStatus.health == 0)
            {
                enemyStatus.dead = true;
                Dead?.Invoke();
            }
            
            saveLoadService.SaveProgress();
        }
    }
}
