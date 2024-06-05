using System;
using System.Collections.Generic;
using DefaultNamespace;
using EnemyScripts;

namespace Data
{
    [Serializable]
    public class LastState
    {
        public string levelToLoad;
        public int waveToLoad;
        public int playerHealth;
        public WrapperOfListOfSpawners wrapperOfListOfSpawners;

        public LastState(string initialLevel, WrapperOfListOfSpawners wrapperOfListOfSpawners)
        {
            levelToLoad = initialLevel;
            waveToLoad = 0;
            this.wrapperOfListOfSpawners = wrapperOfListOfSpawners;
        }
    }
}
