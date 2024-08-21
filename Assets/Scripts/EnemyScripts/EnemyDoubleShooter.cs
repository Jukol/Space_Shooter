using System.Collections;
using Ammo;
using Interfaces;
using UnityEngine;
using Zenject;

namespace EnemyScripts
{
    public class EnemyDoubleShooter : MonoBehaviour, IShootable
    {
        [SerializeField] private Transform[] sockets;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem[] muzzleFlashParticles;
        [SerializeField] private string targetTag;
        [SerializeField] private bool randomFire;

        private IAmmo _ammo;

        private float _fireRate;
        private WaitForSeconds _fireRateYield;
        [Inject (Id = "Enemy")] private IPool _pool;
        private bool _shootStarted;
        [SerializeField] private bool initiated;

        public void Init(float fireRate, int damage, float speed, Transform socket1, ParticleSystem myParticleSystem, Sprite sprite, Transform socket2 = null)
        {
            _fireRate = fireRate;
            _fireRateYield = new WaitForSeconds(fireRate);

            _pool.Upgrade(damage, speed, sprite);
            sockets[0] = socket1;
            sockets[1] = socket2;

            for (int i = 0; i < muzzleFlashParticles.Length; i++)
            {
                muzzleFlashParticles[i] = myParticleSystem;
                muzzleFlashParticles[i].Stop();
            }

            var transform1 = myParticleSystem.transform;
            Transform muzzleTransform1 = transform1;
            muzzleTransform1.position = sockets[0].position;
            
            Transform muzzleTransform2 = transform1;
            muzzleTransform2.position = sockets[1].position;

            Vector2 currentRotation1 = muzzleTransform1.eulerAngles;
            currentRotation1.x = 90;
            muzzleTransform1.eulerAngles = currentRotation1;
            muzzleTransform1.localScale = new Vector2(2f, 2f);
            
            Vector2 currentRotation2 = muzzleTransform2.eulerAngles;
            currentRotation2.x = 90;
            muzzleTransform2.eulerAngles = currentRotation2;
            muzzleTransform2.localScale = new Vector2(2f, 2f);
            
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
                if (randomFire)
                {
                    _fireRateYield = new WaitForSeconds(Random.Range(0f, _fireRate));
                }
                
                yield return _fireRateYield;
                
                for (int i = 0; i < sockets.Length; i++)
                {
                    GameObject bullet = _pool.Request();
                    Bullet bulletComponent = bullet.GetComponent<Bullet>();
                    
                    Transform myTransform = sockets[i].transform;
                    
                    bullet.transform.position = myTransform.position;
                    bullet.transform.rotation = myTransform.rotation;
                    bulletComponent.TargetTag = targetTag;
                    
                    muzzleFlashParticles[i].Play();
                    audioSource.Play();
                }
            }
        }
    }
}
