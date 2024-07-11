using Interfaces;
using UnityEngine;

namespace InputClasses
{
    public class MouseInput : IInput
    {
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public bool IsFire { get; private set; }

        public void UserInput()
        {
            Horizontal = Input.mousePosition.x;
            Vertical = Input.mousePosition.y;
            IsFire = Input.GetMouseButton(0);
        }
    }
}
