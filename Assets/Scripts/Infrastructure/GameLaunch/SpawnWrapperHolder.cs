using System;
using System.Collections.Generic;
using Data;
using EnemyScripts;
using Infrastructure.Wrappers;
using Interfaces;

namespace Infrastructure.GameLaunch
{
    [Serializable]
    public class SpawnWrapperHolder : ISavedProgressWriter
    {
        public ChiefWrapper chiefWrapper = new ();
        
        public SpawnWrapperHolder(SpawnManagerHolder spawnManagerHolder)
        {
            chiefWrapper.SpawnersWrappers = new List<SpawnersWrapper>();
            CreateSpawnersWrappers(spawnManagerHolder);
        }

        private void CreateSpawnersWrappers(SpawnManagerHolder spawnManagerHolder)
        {
            for (int i = 0; i < spawnManagerHolder.SpawnManagers.Length; i++)
            {
                SpawnersWrapper spawnersWrapper = new();
                spawnersWrapper.WrapperOfStatuses = new List<StatusesWrapper>();

                for (int j = 0; j < spawnManagerHolder.SpawnManagers[i].spawners.Length; j++)
                {
                    StatusesWrapper statusesWrapper = new();
                    statusesWrapper.ListOfStatuses = new List<EnemyStatus>();

                    for (int k = 0; k < spawnManagerHolder.SpawnManagers[i].spawners[j].enemyPlaceHolders.Length; k++)
                    {
                        EnemyStatus enemyStatus = new(10, false, 0);
                        statusesWrapper.ListOfStatuses.Add(enemyStatus);
                    }
                    
                    spawnersWrapper.WrapperOfStatuses.Add(statusesWrapper);
                }
                
                chiefWrapper.SpawnersWrappers.Add(spawnersWrapper);
            }
        }

        public void LoadProgress(Progress progress)
        {
            chiefWrapper.SpawnersWrappers = progress.lastState.spawnWrapperHolder.chiefWrapper.SpawnersWrappers;
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lastState.spawnWrapperHolder.chiefWrapper.SpawnersWrappers = chiefWrapper.SpawnersWrappers;
        }
    }
}