using System;
using System.Collections.Generic;
using EnemyScripts;

namespace Infrastructure
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
