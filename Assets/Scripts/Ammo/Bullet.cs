using System.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace Ammo
{
    public class Bullet : MonoBehaviour, IAmmo
    {
        public GameObject Body => gameObject;
        public float Speed => bulletSpeed;
        public float Lifetime => lifetime;
        public int Damage => damage;
        
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float lifetime;
        [SerializeField] private int damage;
        [SerializeField] private GameObject explosion;

        private bool _targetHit;

        private void Awake() => 
            _targetHit = false;

        private void OnEnable()
        {
            transform.parent = null;
            _targetHit = false;
        }

        private void Update()
        {
            if (_targetHit == false) 
                Move();
        }

        private void OnBecameInvisible() => 
            gameObject.SetActive(false);

        private async void OnTriggerEnter2D(Collider2D collision)
        {
            _targetHit = true;
            await DamageAndDie(collision);
        }

        public void Move()
        {
            Transform cachedTransform = transform;
            cachedTransform.position += cachedTransform.up * (Time.deltaTime * bulletSpeed);
        }

        private async Task DamageAndDie(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(damage);
                await ExplosionFx(collision);
                gameObject.SetActive(false);
            }
        }

        private async Task ExplosionFx(Collider2D collision)
        {
            Vector3 position = transform.position;
            position = collision.ClosestPoint(position);
            transform.position = position;

            explosion.SetActive(true);
            explosion.transform.position = position;
            explosion.GetComponent<ParticleSystem>().Play();

            await Task.Delay(100);
        }
    }
}
