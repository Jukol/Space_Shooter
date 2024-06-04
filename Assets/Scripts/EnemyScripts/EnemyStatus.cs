using System;

namespace EnemyScripts
{
    [Serializable]
    public class EnemyStatus
    {
        public EnemyStatus(int spawnerId, int id, int health, bool dead)
        {
            this.spawnerId = spawnerId;
            this.id = id;
            this.health = health;
            this.dead = dead;
        }
        
        public int spawnerId;
        public int id;
        public int health;
        public bool dead;
    }
}
