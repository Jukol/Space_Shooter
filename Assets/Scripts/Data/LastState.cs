using System;
using Infrastructure;
using Infrastructure.Wrappers;

namespace Data
{
    [Serializable]
    public class LastState
    {
        public string levelToLoad;
        public int waveToLoad;
        public int playerHealth;
        public SpawnersWrapper spawnersWrapper;
        public int killCount;

        public LastState(string level, int wave, SpawnersWrapper spawnersWrapper, int playerHealth)
        {
            levelToLoad = level;
            waveToLoad = wave;
            this.spawnersWrapper = spawnersWrapper;
            killCount = 0;
            this.playerHealth = playerHealth;
        }
    }
}
