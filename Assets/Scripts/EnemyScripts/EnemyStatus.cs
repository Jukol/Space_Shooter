using System;

namespace EnemyScripts
{
    [Serializable]
    public class EnemyStatus
    {
        public EnemyStatus(int health, bool dead)
        {
            this.health = health;
            this.dead = dead;
        }

        public int health;
        public bool dead;
    }
}
