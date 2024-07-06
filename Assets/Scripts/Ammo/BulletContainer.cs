using UnityEngine;
namespace Ammo
{
    public class BulletContainer : MonoBehaviour
    {
        [SerializeField] private int capacity;
        
        public int Capacity => capacity;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
