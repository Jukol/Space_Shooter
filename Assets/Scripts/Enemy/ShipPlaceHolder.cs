using UnityEngine;

namespace Enemy
{
    public class ShipPlaceHolder : MonoBehaviour
    {
        public Vector2 Position { get; set; }
        public bool ShipDead { get; set; }
        
        public EnemyShipBehavior Ship { get; set; }

        public void SubscribeToShip()
        {
            //Ship.OnDestroy += () => ShipDead = true;
        }
    }
}
