using EnemyScripts;

namespace Infrastructure.Signals
{
    public class LevelCompleted
    {
        public readonly string LevelToLoad;

        public LevelCompleted(string levelToLoad)
        {
            LevelToLoad = levelToLoad;
        }
    }
}