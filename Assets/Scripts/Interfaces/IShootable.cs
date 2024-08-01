using UnityEngine;

namespace Interfaces
{
    public interface IShootable
    {
        public void Init(float fireRate, int damage, float speed, Transform socket, ParticleSystem myParticleSystem, Sprite sprite);
        public void Shoot();
    }
}
