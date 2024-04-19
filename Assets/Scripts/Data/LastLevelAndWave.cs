using System;
namespace Data
{
    [Serializable]
    public class LastLevelAndWave
    {
        public string levelToLoad;
        public int waveToLoad;

        public LastLevelAndWave(string initialLevel)
        {
            levelToLoad = initialLevel;
            waveToLoad = 0;
        }
    }
}
