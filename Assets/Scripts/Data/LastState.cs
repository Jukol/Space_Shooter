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

        public LastState(string level, int wave, WrapperOfListOfSpawners wrapperOfListOfSpawners)
        {
            levelToLoad = level;
            waveToLoad = wave;
            this.wrapperOfListOfSpawners = wrapperOfListOfSpawners;
        }
    }
}
