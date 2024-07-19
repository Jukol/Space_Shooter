using EnemyScripts;
using Infrastructure.GameLaunch;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class GameLoopState : IPayloadedState<SpawnManager>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly GameInitializer _gameInitializer;
        private int _completedSpawnManagerId;

        public GameLoopState(GameStateMachine gameStateMachine, GameInitializer gameInitializer)
        {
            _gameStateMachine = gameStateMachine;
            _gameInitializer = gameInitializer;
        }

        private void OnLevelCompleted(SpawnManager spawnManager)
        {
            _gameInitializer.ProgressService.Progress.lastState.levelToLoad = "Level " + (_completedSpawnManagerId + 2);
            _gameInitializer.ProgressService.Progress.lastState.spawnManagerIndex = _completedSpawnManagerId + 2;
            _gameInitializer.SaveLoadService.SaveProgress();
            
            _completedSpawnManagerId += 2;
            string nextSceneName = "Level " + _completedSpawnManagerId;

            if (SceneManager.sceneCountInBuildSettings >= _completedSpawnManagerId + 1)
            {
                _gameStateMachine.Enter<LoadLevelState, string>(nextSceneName);
            }
            else
            {
                Debug.Log("Game Over!");
            }
            
        }

        public void Enter(SpawnManager spawnManager)
        {
            _completedSpawnManagerId = spawnManager.id;
            spawnManager.LevelCompleted += OnLevelCompleted;
            spawnManager.Launch();
        }

        public void Exit()
        {
        }
    }
}
