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
        [Inject] private IInput _iInput;
        [Inject] private IPool _pool;
        private bool _shootStarted;


        // private void Update()
        // {
        //     //_iInput.UserInput();
        //     Shoot();
        // }

        public void Init(float fireRate, int damage, float speed)
        {
            muzzleFlashParticles.Stop();
            _fireRateYield = new WaitForSeconds(fireRate);
            _pool.Upgrade(damage, speed);
        }

        public void Shoot()
        {
            // if (_iInput.IsFire && !_shootStarted)
            // {
            //     StartCoroutine(ContinuousShoot());
            // }
            // else if (!_iInput.IsFire && _shootStarted)
            // {
            //     StopAllCoroutines();
            //     _shootStarted = false;
            // }
            
            StartCoroutine(ContinuousShoot());
        }

        private IEnumerator ContinuousShoot()
        {
            //_shootStarted = true;

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
