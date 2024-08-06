using UnityEngine;

namespace Drops
{
    public abstract class Drop : MonoBehaviour
    {
        public float Speed { get => speed; set => speed = value;}

        [SerializeField] private float speed;

        public bool startMoving;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (startMoving)
            {
                MoveDown();
            }
        }

        private void MoveDown()
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
        }

        public abstract void OnTriggerEnter2D(Collider2D other);
    }
}
