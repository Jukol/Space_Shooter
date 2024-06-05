using System;
using DefaultNamespace;

namespace Data
{
    [Serializable]
    public class Progress
    {
        public LastState lastState;
        public WrapperOfListOfSpawners wrapperOfListOfSpawners;

        public Progress(string initialLevel, WrapperOfListOfSpawners wrapperOfListOfSpawners)
        {
            lastState = new LastState(initialLevel, wrapperOfListOfSpawners);
        }
    }
}
