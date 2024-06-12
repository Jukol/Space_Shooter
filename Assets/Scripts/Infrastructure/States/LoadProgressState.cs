using Data;
using DefaultNamespace;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly string _initialLevel;
        private readonly int _initialPlayerHealth;
        private int _initialEnemyHealth;
        private readonly WrapperOfListOfSpawners _wrapperOfListOfSpawners;
        private int _wave;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService, string initialLevel, int initialPlayerHealth, int initialEnemyHealth, WrapperOfListOfSpawners wrapperOfListOfSpawners, int wave)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _initialLevel = initialLevel;
            _initialPlayerHealth = initialPlayerHealth;
            _initialEnemyHealth = initialEnemyHealth;
            _wrapperOfListOfSpawners = wrapperOfListOfSpawners;
            _wave = wave;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.lastState.levelToLoad);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            var progress = _saveLoadService.LoadProgress();
            
            _progressService.Progress =
                progress
                ?? NewProgress();
        }

        private Progress NewProgress()
        {
            return new Progress(_initialLevel, _wave, _wrapperOfListOfSpawners, _initialPlayerHealth);
        }
    }
}
