using EnemyScripts;
using Infrastructure.GameLaunch;
using Infrastructure.Signals;
using Interfaces;
using PlayerScripts;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.States
{
    public class GameLoopState : IPayloadedState2<SpawnManager, Player, StartMenuController>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly GameInitializer _gameInitializer;
        private readonly SignalBus _signalBus;
        private readonly SceneLoader _sceneLoader;
        
        private int _completedSpawnManagerId;

        public GameLoopState(
            GameStateMachine gameStateMachine, 
            GameInitializer gameInitializer,
            SignalBus signalBus,
            SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _gameInitializer = gameInitializer;
            _signalBus = signalBus;
            _sceneLoader = sceneLoader;
        }

        private void OnLevelCompleted(SpawnManager spawnManager)
        {
            _gameInitializer.ProgressService.Progress.lastState.levelToLoad = "Level " + (_completedSpawnManagerId + 2);
            int wave = _gameInitializer.ProgressService.Progress.lastState.waveToLoad = 0;
            
            _signalBus.Fire(new WaveCompleted(wave));
            
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
                GameOverRoutine();
            }
            
        }

        private void GameOverRoutine()
        {
            Debug.Log("Game Over!");
        }

        public void Enter(SpawnManager spawnManager, Player player, StartMenuController startMenuController)
        {
            _completedSpawnManagerId = spawnManager.id;
            spawnManager.LevelCompleted += OnLevelCompleted;
            player.GetToStartPosition();
            
            startMenuController.Init(spawnManager, player, _gameInitializer, _gameStateMachine);
        }

        public void Exit()
        {
        }
    }
}
