namespace Interfaces
{
    public interface IShootable
    {
        public void Init(float fireRate, int damage, float speed);
        public void Shoot();
    }
}
