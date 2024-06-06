using System;
using DefaultNamespace;

namespace Data
{
    [Serializable]
    public class Progress
    {
        public LastState lastState;
        public WrapperOfListOfSpawners wrapperOfListOfSpawners;

        public Progress(string level, int wave, WrapperOfListOfSpawners wrapperOfListOfSpawners)
        {
            lastState = new LastState(level, wave, wrapperOfListOfSpawners);
        }
    }
}
