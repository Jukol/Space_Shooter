using System;
using Infrastructure.GameLaunch;

namespace Data
{
    [Serializable]
    public class Progress
    {
        public LastState lastState;

        public Progress(
            string level, 
            int wave,
            SpawnWrapperHolder spawnWrapperHolder,
            int spawnManagerIndex, 
            int playerHealth)
        {
            lastState = new LastState(level, wave, spawnWrapperHolder,  spawnManagerIndex, playerHealth);
        }
    }
}
