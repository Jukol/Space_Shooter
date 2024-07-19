using Infrastructure.Signals;
using Interfaces;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Boot = "Boot";
        
        private readonly SceneLoader _sceneLoader;
        private readonly GameStateMachine _gameStateMachine;

        public BootstrapState(SceneLoader sceneLoader, GameStateMachine gameStateMachine)
        {
            _sceneLoader = sceneLoader;
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter()
        {
            _sceneLoader.Load(Boot, onLoaded: EnterLoadProgressState);
        }

        public void Exit()
        {

        }

        private void EnterLoadProgressState()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }
    }
}
