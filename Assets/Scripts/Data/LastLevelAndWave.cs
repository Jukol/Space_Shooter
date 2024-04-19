using System;
namespace Data
{
    [Serializable]
    public class LastProgress
    {
        public string levelToLoad;
        public int waveToLoad;

        public LastProgress(string initialLevel)
        {
            levelToLoad = initialLevel;
            waveToLoad = 0;
        }
    }
}
