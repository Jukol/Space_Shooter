using DefaultNamespace;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.States;
using Logic;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner,
            LoadingCurtain curtain,
            string initialLevel,
            int initialPlayerHealth,
            int initialEnemyHealth,
            SpawnersWrapper spawnersWrapper,
            int wave,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService,
            IGameFactory gameFactory)
        {
            StateMachine = new GameStateMachine(
                new SceneLoader(coroutineRunner),
                curtain,
                initialLevel,
                initialPlayerHealth,
                initialEnemyHealth,
                spawnersWrapper,
                wave,
                progressService,
                saveLoadService,
                gameFactory);
        }
    }
}
