using System;
using Drops;
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
        public bool hasDrop;

        private IGameFactory _gameFactory;
        private UpgradeDrop _upgradeDrop;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            if (hasDrop)
            {
                _upgradeDrop = _gameFactory.CreateUpgradeDrop();
                _upgradeDrop.gameObject.SetActive(false);
            }
        }

        public void UpdateStatus(float health, ISaveLoadService saveLoadService, bool isHit)
        {
            enemyStatus.health = health;
            enemyStatus.hit = isHit;
            if (enemyStatus.health <= 0)
            {
                enemyStatus.dead = true;

                if (hasDrop)
                {
                    _upgradeDrop.gameObject.SetActive(true);
                    _upgradeDrop.transform.position = transform.position;
                    _upgradeDrop.startMoving = true;
                }
            } 

            saveLoadService.SaveProgress();
        }
    }
}
