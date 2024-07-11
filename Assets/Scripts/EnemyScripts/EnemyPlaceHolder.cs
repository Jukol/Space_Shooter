using System;
using Interfaces;
using UnityEngine;
using Zenject;

namespace EnemyScripts
{
    [Serializable]
    public class EnemyPlaceHolder : MonoBehaviour
    {
        public Vector2 Position { get; set; }
        public EnemyStatus enemyStatus;

        public void UpdateStatus(int health, ISaveLoadService saveLoadService)
        {
            enemyStatus.health = health;
            if (enemyStatus.health == 0) 
                enemyStatus.dead = true;

            saveLoadService.SaveProgress();
        }
    }
}
