using System;
using System.Collections.Generic;
using DefaultNamespace;
using EnemyScripts;

namespace Data
{
    [Serializable]
    public class LastState
    {
        public string levelToLoad;
        public int waveToLoad;
        public int playerHealth;
        public OverallEnemyStatuses overallEnemyStatuses;

        public LastState(string initialLevel, OverallEnemyStatuses overallEnemyStatuses)
        {
            levelToLoad = initialLevel;
            waveToLoad = 0;
            this.overallEnemyStatuses = overallEnemyStatuses;
        }
    }
}
