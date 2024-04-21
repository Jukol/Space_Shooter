using System;
namespace Data
{
    [Serializable]
    public class LastState
    {
        public string levelToLoad;
        public int waveToLoad;
        public int playerHealth;

        public LastState(string initialLevel, int health)
        {
            levelToLoad = initialLevel;
            waveToLoad = 0;
            playerHealth = health;
        }
    }
}
