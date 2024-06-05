using System;
using System.Collections.Generic;
using EnemyScripts;

namespace DefaultNamespace
{
    [Serializable]
    public class WrapperOfListOfSpawners
    {
        public List<WrapperOfListOfStatuses> WrapperOfStatuses;
    }
    
    [Serializable]
    public class WrapperOfListOfStatuses
    {
        public List<EnemyStatus> ListOfStatuses;
    }
}
