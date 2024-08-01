using System.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace Ammo
{
    public class Bullet : MonoBehaviour, IAmmo
    {
        public GameObject Body => gameObject;
        public float Speed => _speed;
        public float Lifetime => lifetime;
        public int Damage => _damage;
        
        private float _speed;
        private int _damage;
        
        [SerializeField] private float lifetime;
        [SerializeField] private GameObject explosion;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool _targetHit;

        public void Init(int damage, float speed, Sprite sprite)
        {
            _damage = damage;
            _speed = speed;
            _targetHit = false;
            spriteRenderer.sprite = sprite;
        }

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

        public void Move()
        {
            Transform cachedTransform = transform;
            cachedTransform.position += cachedTransform.up * (Time.deltaTime * _speed);
        }

        private void OnBecameInvisible() => 
            gameObject.SetActive(false);

        private async void OnTriggerEnter2D(Collider2D collision)
        {
            _targetHit = true;
            await DamageAndDie(collision);
        }

        private async Task DamageAndDie(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(_damage);
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
