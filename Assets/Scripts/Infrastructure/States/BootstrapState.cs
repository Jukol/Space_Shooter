using Infrastructure.Signals;
using Interfaces;
using Zenject;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Boot = "Boot";
        
        private readonly SceneLoader _sceneLoader;
        private readonly SignalBus _signalBus;

        public BootstrapState(SceneLoader sceneLoader, SignalBus signalBus)
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
            _signalBus.Fire<BootstrapLoaded>();
        }
    }
}
