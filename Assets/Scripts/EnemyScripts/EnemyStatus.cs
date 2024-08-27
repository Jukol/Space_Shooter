using System;

namespace EnemyScripts
{
    [Serializable]
    public class EnemyStatus
    {
        public float health;
        public bool dead;
        public EnemyUpgradeData enemyUpgradeData;
        public bool hit;
        
        public EnemyStatus(float health, bool dead, EnemyUpgradeData enemyUpgradeData, bool isHit)
        {
            this.health = health;
            this.dead = dead;
            this.enemyUpgradeData = enemyUpgradeData;
            hit = isHit;
        }
    }
}
