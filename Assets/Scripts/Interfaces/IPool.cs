using UnityEngine;

namespace Interfaces
{
    public interface IPool : IService
    {
        public GameObject Request();
        public void Upgrade(int damage, float speed, Sprite sprite);
    }
}
