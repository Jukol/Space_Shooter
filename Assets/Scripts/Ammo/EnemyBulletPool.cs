using System.Collections.Generic;
using EnemyScripts;
using Interfaces;
using UnityEngine;

namespace Ammo
{
    public class EnemyBulletPool : IPool
    {
        private readonly List<GameObject> _ammoBatch = new();
        private readonly EnemyBulletContainer _enemyBulletContainer;
        private readonly int _capacity;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly EnemyUpgradeDataList _enemyUpgradeDataList;

        private int _damage;
        private float _speed;
        private Sprite _sprite;

        public EnemyBulletPool(
            IGameFactory gameFactory, 
            EnemyBulletContainer enemyBulletContainer, 
            IPersistentProgressService persistentProgressService, 
            EnemyUpgradeDataList enemyUpgradeDataList)
        {
            _gameFactory = gameFactory;
            _enemyBulletContainer = enemyBulletContainer;
            _capacity = _enemyBulletContainer.Capacity;
            _enemyUpgradeDataList = enemyUpgradeDataList;
            _progressService = persistentProgressService;

            int upgradeLevel = _progressService.Progress.lastState.playerUpgradeLevel;
            int ship = _progressService.Progress.lastState.playerShip;
            
            _damage = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].bulletDamage;
            _speed = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].bulletSpeed;
            _sprite = _enemyUpgradeDataList.shipUpgrades[0].enemyUpgrades[0].bulletSprite;
            
            Generate();
        }

        public GameObject Request()
        {
            foreach (GameObject ammo in _ammoBatch)
            {
                if (NotAvailable(ammo) || AlreadyInHierarchy(ammo)) continue;
                
                MakeReady(ammo);

                return ammo;
            }
            
            return Add();
        }
        
        public void Upgrade(int damage, float speed, Sprite sprite)
        {
            _enemyBulletContainer.Clean();
            
            _damage = damage;
            _speed = speed;

            foreach (Transform bullet in _enemyBulletContainer.transform)
            {
                bullet.GetComponent<Bullet>().Init(damage, speed, sprite);
            }
        }

        private void Generate()
        {
            for (int i = 0; i < _capacity; i++) 
                Add();
        }

        private GameObject Add()
        {
            GameObject ammo = _gameFactory.CreateBullet(_damage, _speed, _sprite);
            ammo.transform.SetParent(_enemyBulletContainer.transform);
            ammo.SetActive(false);
            _ammoBatch.Add(ammo);
            return ammo;
        }

        private void MakeReady(GameObject ammo)
        {

            ammo.SetActive(true);
            ammo.transform.SetParent(_enemyBulletContainer.transform);
        }

        private static bool AlreadyInHierarchy(GameObject ammo)
        {
            if (ammo.activeInHierarchy) return true;
            return false;
        }

        private static bool NotAvailable(GameObject ammo)
        {
            if (!ammo) return true;
            return false;
        }
    }
}
