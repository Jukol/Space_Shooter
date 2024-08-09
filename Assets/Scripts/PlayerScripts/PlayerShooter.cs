using System.Collections;
using Interfaces;
using UnityEngine;
using Zenject;

namespace PlayerScripts
{
    public class PlayerShooter : MonoBehaviour, IShootable
    {
        [SerializeField] private Transform socket;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem muzzleFlashParticles;

        private IAmmo _ammo;

        private WaitForSeconds _fireRateYield;
        [Inject (Id = "Player")] private IPool _pool;
        private bool _shootStarted;
        [SerializeField] private bool _initiated = false;

        public void Init(float fireRate, int damage, float speed, Transform newSocket, ParticleSystem myParticleSystem, Sprite sprite)
        {
            _fireRateYield = new WaitForSeconds(fireRate);
            _pool.Upgrade(damage, speed, sprite);
            socket = newSocket;

            muzzleFlashParticles = myParticleSystem;
            muzzleFlashParticles.Stop();

            Transform muzzleTransform = myParticleSystem.transform;
            muzzleTransform.position = socket.position;
            
            Vector2 currentRotation = muzzleTransform.eulerAngles;
            currentRotation.x = 90;
            muzzleTransform.eulerAngles = currentRotation;
            muzzleTransform.localScale = new Vector2(2f, 2f);
            
            _initiated = true;
        }

        public void Shoot()
        {
            if (_initiated)
            {
                StartCoroutine(ContinuousShoot());
            }
        }
        
        public void StopShooting()
        {
            StopAllCoroutines();
        }

        private IEnumerator ContinuousShoot()
        {
            while (true)
            {
                muzzleFlashParticles.Play();
                audioSource.Play();

                GameObject bullet = _pool.Request();

                Transform myTransform = socket.transform;

                bullet.transform.position = myTransform.position;
                bullet.transform.rotation = myTransform.rotation;

                yield return _fireRateYield;
            }
        }
    }
}
