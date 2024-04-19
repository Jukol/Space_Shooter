using System.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace Ammo
{
    public class Bullet : MonoBehaviour, IAmmo
    {
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float lifetime;
        [SerializeField] private int damage;
        [SerializeField] private GameObject explosion;

        private bool _targetHit;

        private void Awake()
        {
            _targetHit = false;
        }

        private void Update()
        {
            if (_targetHit == false)
            {
                Move();
            }
        }

        private void OnEnable()
        {
            transform.parent = null;
            _targetHit = false;
        }

        private void OnBecameInvisible()
        {
            gameObject.SetActive(false);
        }

        private async void OnTriggerEnter2D(Collider2D collision)
        {
            _targetHit = true;

            IDamageable obj = collision.GetComponent<IDamageable>();
            if (obj != null)
            {
                obj.Damage(damage);
                Vector3 position = transform.position;
                position = collision.ClosestPoint(position);
                transform.position = position;

                explosion.SetActive(true);
                explosion.transform.position = position;
                explosion.GetComponent<ParticleSystem>().Play();

                await Task.Delay(100);

                gameObject.SetActive(false);
            }
        }

        public GameObject Body => gameObject;
        public float Speed => bulletSpeed;
        public float Lifetime => lifetime;
        public int Damage => damage;

        public void Move()
        {
            Transform transform1 = transform;
            transform1.position += transform1.up * (Time.deltaTime * bulletSpeed);
        }
    }
}
