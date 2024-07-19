using EnemyScripts;
using Infrastructure.GameLaunch;
using Infrastructure.Signals;
using Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.States
{
    public class GameLoopState : IPayloadedState<LevelLoadLoaded>
    {
        private readonly GameStateMachine _gameStateMachine;
        private int _completedSpawnManagerId;

        public GameLoopState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void OnLevelCompleted()
        {
            _completedSpawnManagerId += 2;
            string nextSceneName = "Level " + _completedSpawnManagerId;
            _gameStateMachine.Enter<LoadLevelState, string>(nextSceneName);
        }

        public void Enter(LevelLoadLoaded payload)
        {
            SpawnManager spawnManager = payload.SpawnManager;
            _completedSpawnManagerId = spawnManager.id;
            spawnManager.LevelCompleted += OnLevelCompleted;
            spawnManager.Launch();
        }

        public void Exit()
        {
        }
    }
}
