using EnemyScripts;

namespace Infrastructure.Signals
{
    public class LevelLoadLoaded
    {
        public SpawnManager SpawnManager { get; private set; }
        
        public LevelLoadLoaded(SpawnManager spawnManager)
        {
            SpawnManager = spawnManager;
        }
    }
}