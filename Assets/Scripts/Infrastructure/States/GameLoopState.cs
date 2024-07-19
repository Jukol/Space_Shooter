using EnemyScripts;
using Interfaces;

namespace Infrastructure.States
{
    public class GameLoopState : IPayloadedState<SpawnManager>
    {
        private readonly GameStateMachine _gameStateMachine;
        private int _completedSpawnManagerId;

        public GameLoopState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void OnLevelCompleted(SpawnManager spawnManager)
        {
            spawnManager.DestroyMe();
            _completedSpawnManagerId += 2;
            string nextSceneName = "Level " + _completedSpawnManagerId;
            _gameStateMachine.Enter<LoadLevelState, string>(nextSceneName);
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
