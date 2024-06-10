using System;
using System.Collections.Generic;
using DefaultNamespace;
using EnemyScripts;
using HUD;

namespace Data
{
    [Serializable]
    public class LastState
    {
        public string levelToLoad;
        public int waveToLoad;
        public int playerHealth;
        public WrapperOfListOfSpawners wrapperOfListOfSpawners;
        public int killCount;

        public LastState(string level, int wave, WrapperOfListOfSpawners wrapperOfListOfSpawners)
        {
            levelToLoad = level;
            waveToLoad = wave;
            this.wrapperOfListOfSpawners = wrapperOfListOfSpawners;
            killCount = 0;
        }
    }
}
