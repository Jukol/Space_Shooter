using Data;
using Infrastructure.GameLaunch;
using Infrastructure.Signals;
using Interfaces;

namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameInitializer _gameInitializer;
        private readonly GameStateMachine _gameStateMachine;
        private SpawnWrapperHolder _spawnWrapperHolder;
        
        public LoadProgressState(GameInitializer gameInitializer, GameStateMachine gameStateMachine)
        {
            _gameInitializer = gameInitializer;
            _gameStateMachine = gameStateMachine;
            _spawnWrapperHolder = _gameInitializer.GameFactory.CreateSpawnWrapperHolder();
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameInitializer.GameFactory.CleanUp();
            _gameStateMachine.Enter<LoadLevelState, string>(_gameInitializer.ProgressService.Progress.lastState.levelToLoad);
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
                _spawnWrapperHolder,
                _gameInitializer.SpawnManagerIndex,
                _gameInitializer.InitialPlayerHealth,
                _gameInitializer.InitialPlayerUpgradeLevel);
        }
    }
}
