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
        private Drop _drop;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            if (hasDrop)
            {
                int random = UnityEngine.Random.Range(0, 2);
                if (random == 0)
                {
                    _drop = _gameFactory.CreateUpgradeDrop();
                    _drop.gameObject.SetActive(false);
                }
                else
                {
                    _drop = _gameFactory.CreateHealthDrop();
                    _drop.gameObject.SetActive(false);
                }
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
                    _drop.gameObject.SetActive(true);
                    _drop.transform.position = transform.position;
                    _drop.startMoving = true;
                }
            } 

            saveLoadService.SaveProgress();
        }
    }
}
