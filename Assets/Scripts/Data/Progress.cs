using System;
using Infrastructure.GameLaunch;

namespace Data
{
    [Serializable]
    public class Progress
    {
        public LastState lastState;

        public Progress(
            string level, 
            int wave,
            SpawnWrapperHolder spawnWrapperHolder,
            int spawnManagerIndex, 
            int playerHealth,
            int playerShip,
            int playerUpgradeLevel,
            int enemyShip)
        {
            lastState = new LastState(level, wave, spawnWrapperHolder, spawnManagerIndex, playerHealth, playerShip, playerUpgradeLevel, enemyShip);
        }
    }
}
