using Data;
using Infrastructure.GameLaunch;
using Infrastructure.Signals;
using Interfaces;
using Zenject;

namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameInitializer _gameInitializer;
        private SignalBus _signalBus;

        public LoadProgressState(GameInitializer gameInitializer, SignalBus signalBus)
        {
            _gameInitializer = gameInitializer;
            _signalBus = signalBus;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _signalBus.Fire<ProgressLoaded>();
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            var progress = _gameInitializer.SaveLoadService.LoadProgress();
            
            _gameInitializer.ProgressService.Progress = progress ?? NewProgress();
        }

        private Progress NewProgress()
        {
            return new Progress(
                _gameInitializer.InitialLevel, 
                _gameInitializer.InitialWave, 
                _gameInitializer.SpawnersWrapper, 
                _gameInitializer.InitialPlayerHealth);
        }
    }
}
