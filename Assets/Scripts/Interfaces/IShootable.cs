using UnityEngine;

namespace Interfaces
{
    public interface IShootable
    {
        public void Init(float fireRate, int damage, float speed, Transform socket1, ParticleSystem myParticleSystem, Sprite sprite, Transform socket2 = null);
        public void Shoot();
        public void StopShooting();
    }
}
