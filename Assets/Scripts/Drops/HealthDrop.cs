using PlayerScripts;
using UnityEngine;

namespace Drops
{
    public class HealthDrop : Drop
    {
        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player>().UpgradeHealth();
                Destroy(gameObject);
            }
        }
    }
}
