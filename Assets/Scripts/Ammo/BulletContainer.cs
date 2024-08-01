using UnityEngine;
namespace Ammo
{
    public class BulletContainer : MonoBehaviour
    {
        [SerializeField] private int capacity;
        
        public int Capacity => capacity;

        public void Clean()
        {
            foreach (Transform bullet in transform)
            {
                Destroy(bullet.gameObject);
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
