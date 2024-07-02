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
        [Inject] private ISaveLoadService _saveLoadService;

        public void UpdateStatus(int health)
        {
            enemyStatus.health = health;
            if (enemyStatus.health == 0)
            {
                enemyStatus.dead = true;
                Dead?.Invoke();
            }
            
            _saveLoadService.SaveProgress();
        }
    }
}
