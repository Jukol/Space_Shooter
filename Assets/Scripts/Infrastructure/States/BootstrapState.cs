using Infrastructure.Signals;
using Interfaces;
using Zenject;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Boot = "Boot";
        private readonly SignalBus _signalBus;
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(
            SceneLoader sceneLoader,
            SignalBus signalBus)
        {
            _sceneLoader = sceneLoader;
            _signalBus = signalBus;
        }
        
        public void Enter()
        {
            _sceneLoader.Load(Boot, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {

        }

        private void EnterLoadLevel()
        {
            //_gameStateMachine.Enter<LoadProgressState>();
            _signalBus.Fire<BootstrapLoaded>();
        }
    }
}
