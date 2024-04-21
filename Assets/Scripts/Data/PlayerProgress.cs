using System;
namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public LastState lastState;

        public PlayerProgress(string initialLevel, int health)
        {
            lastState = new LastState(initialLevel, health);
        }
    }
}
