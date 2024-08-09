using UnityEngine;
namespace Ammo
{
    public class PlayerBulletContainer : MonoBehaviour
    {
        [SerializeField] private int capacity;
        
        public int Capacity => capacity;

        public void Clean()
        {
            foreach (Transform bullet in transform)
            {
                bullet.gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
