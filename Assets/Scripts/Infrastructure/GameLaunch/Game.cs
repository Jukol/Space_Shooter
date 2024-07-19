using Infrastructure.States;
using Interfaces;
using Zenject;

namespace Infrastructure.GameLaunch
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(GameInitializer gameInitializer, ICoroutineRunner coroutineRunner, SignalBus signalBus)
        {
            StateMachine = new GameStateMachine(gameInitializer, coroutineRunner);
        }
    }
}
