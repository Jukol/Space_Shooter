using System.Collections;
using Interfaces;
using UnityEngine;
using Zenject;

namespace PlayerScripts
{
    public class Shooter : MonoBehaviour, IShootable
    {
        [SerializeField] private Transform socket;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem muzzleFlashParticles;

        private IAmmo _ammo;

        private WaitForSeconds _fireRateYield;
        [Inject] private IPool _pool;
        private bool _shootStarted;

        public void Init(float fireRate, int damage, float speed, Transform newSocket, ParticleSystem myParticleSystem)
        {
            _fireRateYield = new WaitForSeconds(fireRate);
            _pool.Upgrade(damage, speed);
            socket = newSocket;

            muzzleFlashParticles = myParticleSystem;
            muzzleFlashParticles.Stop();

            Transform muzzleTransform = myParticleSystem.transform;
            muzzleTransform.position = socket.position;
            
            Vector2 currentRotation = muzzleTransform.eulerAngles;
            currentRotation.x = 90;
            muzzleTransform.eulerAngles = currentRotation;
            muzzleTransform.localScale = new Vector2(2f, 2f);
        }

        public void Shoot()
        {
           StartCoroutine(ContinuousShoot());
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
