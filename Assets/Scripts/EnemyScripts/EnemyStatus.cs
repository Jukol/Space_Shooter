using System;

namespace EnemyScripts
{
    [Serializable]
    public class EnemyStatus
    {
        public int health;
        public bool dead;
        public int ship;
        
        public EnemyStatus(int health, bool dead, int ship)
        {
            this.health = health;
            this.dead = dead;
            this.ship = ship;
        }
    }
}
