using System;
using Infrastructure;
using Infrastructure.GameLaunch;
using Infrastructure.Wrappers;

namespace Data
{
    [Serializable]
    public class LastState
    {
        public string levelToLoad;
        public int waveToLoad;
        public int playerHealth;
        public int spawnManagerIndex;
        public SpawnWrapperHolder spawnWrapperHolder;
        public int killCount;

        public LastState(
            string level, 
            int wave,
            SpawnWrapperHolder spawnWrapperHolder,
            int spawnManagerIndex,
            int playerHealth)
        {
            levelToLoad = level;
            waveToLoad = wave;
            this.spawnWrapperHolder = spawnWrapperHolder;
            this.spawnManagerIndex = spawnManagerIndex;
            killCount = 0;
            this.playerHealth = playerHealth;
        }
    }
}
