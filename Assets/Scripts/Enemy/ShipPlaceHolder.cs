using UnityEngine;

namespace Enemy
{
    public class ShipPlaceHolder : MonoBehaviour
    {
        [SerializeField] private bool shipDead;
        public Vector2 Position { get; set; }
        public bool ShipDead { get => shipDead; set => shipDead = value; }
    }
}
