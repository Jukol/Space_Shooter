using Data;
using Infrastructure.Signals;
using Interfaces;
using Zenject;

namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly string _initialLevel;
        private readonly int _initialPlayerHealth;
        private readonly SpawnersWrapper _spawnersWrapper;
        private int _wave;
        private SignalBus _signalBus;

        public LoadProgressState(
            IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService, 
            string initialLevel, 
            int initialPlayerHealth, 
            SpawnersWrapper spawnersWrapper, 
            int wave,
            SignalBus signalBus)
        {
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _initialLevel = initialLevel;
            _initialPlayerHealth = initialPlayerHealth;
            _spawnersWrapper = spawnersWrapper;
            _wave = wave;
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
            var progress = _saveLoadService.LoadProgress();
            
            _progressService.Progress = progress ?? NewProgress();
        }

        private Progress NewProgress()
        {
            return new Progress(_initialLevel, _wave, _spawnersWrapper, _initialPlayerHealth);
        }
    }
}
