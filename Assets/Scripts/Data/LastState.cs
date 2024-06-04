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
        public List<ListOfSpawnerEnemyStatusLists> listOfSpawnerEnemyStatusLists;

        public LastState(string initialLevel)
        {
            levelToLoad = initialLevel;
            waveToLoad = 0;
            listOfSpawnerEnemyStatusLists = new();
        }
    }
}
