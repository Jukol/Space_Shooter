using System;
using DefaultNamespace;

namespace Data
{
    [Serializable]
    public class Progress
    {
        public LastState lastState;

        public Progress(string level, int wave, WrapperOfListOfSpawners wrapperOfListOfSpawners, int playerHealth)
        {
            lastState = new LastState(level, wave, wrapperOfListOfSpawners, playerHealth);
        }
    }
}
