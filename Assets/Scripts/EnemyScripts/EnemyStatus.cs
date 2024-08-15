using System;

namespace EnemyScripts
{
    [Serializable]
    public class EnemyStatus
    {
        public int health;
        public bool dead;
        public EnemyUpgradeData enemyUpgradeData;
        
        public EnemyStatus(int health, bool dead, EnemyUpgradeData enemyUpgradeData)
        {
            this.health = health;
            this.dead = dead;
            this.enemyUpgradeData = enemyUpgradeData;
        }
    }
}
