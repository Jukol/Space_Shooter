using Infrastructure.States;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Infrastructure.GameLaunch
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private GameInitializer _gameInitializer;
        private Game _game;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(GameInitializer gameInitializer, SignalBus signalBus)
        {
            _gameInitializer = gameInitializer;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _game = new Game(_gameInitializer, this, _signalBus);
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}
