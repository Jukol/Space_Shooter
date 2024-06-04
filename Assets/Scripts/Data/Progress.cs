using System;

namespace Data
{
    [Serializable]
    public class Progress
    {
        public LastState lastState;

        public Progress(string initialLevel)
        {
            lastState = new LastState(initialLevel);
        }
    }
}
