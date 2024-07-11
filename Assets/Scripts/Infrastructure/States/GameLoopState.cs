using Infrastructure.GameLaunch;
using Interfaces;

namespace Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameInitializer _gameInitializer;
        public GameLoopState(GameInitializer gameInitializer)
        {
            _gameInitializer = gameInitializer;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}
