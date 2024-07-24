using EnemyScripts;
using Infrastructure.GameLaunch;
using Infrastructure.Signals;
using Interfaces;
using PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.States
{
    public class GameLoopState : IPayloadedState2<SpawnManager, Player, StartMenuHandler>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly GameInitializer _gameInitializer;
        private readonly SignalBus _signalBus;
        
        private int _completedSpawnManagerId;

        public GameLoopState(
            GameStateMachine gameStateMachine, 
            GameInitializer gameInitializer,
            SignalBus signalBus)
        {
            _gameStateMachine = gameStateMachine;
            _gameInitializer = gameInitializer;
            _signalBus = signalBus;
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
            PlayerPrefs.DeleteAll();
        }

        public void Enter(SpawnManager spawnManager, Player player, StartMenuHandler startMenuHandler)
        {
            _completedSpawnManagerId = spawnManager.id;
            spawnManager.LevelCompleted += OnLevelCompleted;
            
            startMenuHandler.Init(spawnManager, player);
        }

        public void Exit()
        {
        }
    }
}
