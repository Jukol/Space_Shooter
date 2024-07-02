using System.Collections.Generic;
using Infrastructure.Factory;
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

        public BulletPool(IGameFactory gameFactory, BulletContainer bulletContainer)
        {
            _gameFactory = gameFactory;
            _bulletContainer = bulletContainer;
            _capacity = _bulletContainer.Capacity;
            Generate();
        }

        public void Generate()
        {
            for (int i = 0; i < _capacity; i++) 
                Add();
        }

        public GameObject Add()
        {
            GameObject ammo = _gameFactory.CreateBullet();
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
