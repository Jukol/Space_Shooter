using System;
using System.Collections.Generic;
using EnemyScripts;

namespace DefaultNamespace
{
    [Serializable]
    public class SpawnersWrapper
    {
        public List<StatusesWrapper> WrapperOfStatuses;
    }
    
    [Serializable]
    public class StatusesWrapper
    {
        public List<EnemyStatus> ListOfStatuses;
    }
}
