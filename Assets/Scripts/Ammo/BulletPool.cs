using System.Collections.Generic;
using Infrastructure.Factory;
using Interfaces;
using PlayerScripts;
using UnityEngine;
using Zenject;

namespace Ammo
{
    public class BulletPool : IPool
    {
        private readonly List<GameObject> _ammoBatch = new();
        private readonly BulletContainer _bulletContainer;
        private readonly int _capacity;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly PlayerUpgradeDataList _playerUpgradeDataList;

        private int _damage;
        private float _speed;

        public BulletPool(IGameFactory gameFactory, BulletContainer bulletContainer, IPersistentProgressService persistentProgressService, PlayerUpgradeDataList playerUpgradeDataList)
        {
            _gameFactory = gameFactory;
            _bulletContainer = bulletContainer;
            _capacity = _bulletContainer.Capacity;
            _playerUpgradeDataList = playerUpgradeDataList;
            _progressService = persistentProgressService;

            int upgradeLevel = _progressService.Progress.lastState.playerUpgradeLevel;
            _damage = _playerUpgradeDataList.playerUpgrades[upgradeLevel].bulletDamage;
            _speed = _playerUpgradeDataList.playerUpgrades[upgradeLevel].bulletSpeed;
            
            Generate();
        }

        private void Generate()
        {
            for (int i = 0; i < _capacity; i++) 
                Add();
        }

        private GameObject Add()
        {
            GameObject ammo = _gameFactory.CreateBullet(_damage, _speed);
            ammo.transform.SetParent(_bulletContainer.transform);
            ammo.SetActive(false);
            _ammoBatch.Add(ammo);
            return ammo;
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

        private void MakeReady(GameObject ammo)
        {

            ammo.SetActive(true);
            ammo.transform.SetParent(_bulletContainer.transform);
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
