using System.Collections;
using Ammo;
using Interfaces;
using UnityEngine;
using Zenject;

namespace EnemyScripts
{
    public class EnemyShooter : MonoBehaviour, IShootable
    {
        [SerializeField] private Transform socket;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem muzzleFlashParticles;
        [SerializeField] private string targetTag;
        [SerializeField] private bool randomFire;

        private IAmmo _ammo;

        private float _fireRate;
        private WaitForSeconds _fireRateYield;
        [Inject (Id = "Enemy")] private IPool _pool;
        private bool _shootStarted;
        [SerializeField] private bool initiated;

        public void Init(float fireRate, int damage, float speed, Transform newSocket, ParticleSystem myParticleSystem, Sprite sprite, Transform socket2 = null)
        {
            _fireRate = fireRate;
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
            
            initiated = true;
        }

        public void Shoot()
        {
            if (initiated)
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
                Bullet bulletComponent = bullet.GetComponent<Bullet>();

                Transform myTransform = socket.transform;

                bullet.transform.position = myTransform.position;
                bullet.transform.rotation = myTransform.rotation;
                bulletComponent.TargetTag = targetTag;
                
                if (randomFire)
                {
                    _fireRateYield = new WaitForSeconds(Random.Range(0f, _fireRate));
                }
                
                yield return _fireRateYield;
            }
        }
    }
}
