using System;
using DefaultNamespace;

namespace Data
{
    [Serializable]
    public class Progress
    {
        public LastState lastState;
        public OverallEnemyStatuses overallEnemyStatuses;

        public Progress(string initialLevel, OverallEnemyStatuses overallEnemyStatuses)
        {
            lastState = new LastState(initialLevel, overallEnemyStatuses);
        }
    }
}
