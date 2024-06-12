using System;
using DefaultNamespace;

namespace Data
{
    [Serializable]
    public class Progress
    {
        public LastState lastState;

        public Progress(string level, int wave, SpawnersWrapper spawnersWrapper, int playerHealth)
        {
            lastState = new LastState(level, wave, spawnersWrapper, playerHealth);
        }
    }
}
