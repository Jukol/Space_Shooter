using System;
namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public LastLevelAndWave lastLevelAndWave;

        public PlayerProgress(string initialLevel)
        {
            lastLevelAndWave = new LastLevelAndWave(initialLevel);
        }
    }
}
