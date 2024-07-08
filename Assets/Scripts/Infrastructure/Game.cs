using Infrastructure.Factory;
using Infrastructure.States;
using Interfaces;
using Logic;
using Zenject;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner,
            LoadingCurtain curtain,
            string initialLevel,
            int initialPlayerHealth,
            SpawnersWrapper spawnersWrapper,
            int wave,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService,
            IGameFactory gameFactory,
            SignalBus signalBus)
        {
            StateMachine = new GameStateMachine(
                new SceneLoader(coroutineRunner),
                curtain,
                initialLevel,
                initialPlayerHealth,
                spawnersWrapper,
                wave,
                progressService,
                saveLoadService,
                gameFactory,
                signalBus);
        }
    }
}
