using EnemyScripts;
using Infrastructure.Factory;
using Infrastructure.GameLaunch;
using Infrastructure.Signals;
using Interfaces;
using Logic;
using PlayerScripts;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IPersistentProgressService _progressService;
        private readonly SceneLoader _sceneLoader;
        private readonly GameInitializer _gameInitializer;
        private readonly GameStateMachine _gameStateMachine;
        
        private SpawnManager _spawnManager;

        private int enterCheck;

        public LoadLevelState(GameInitializer gameInitializer, ICoroutineRunner coroutineRunner, GameStateMachine gameStateMachine)
        {
            _gameInitializer = gameInitializer;
            _sceneLoader = new SceneLoader(coroutineRunner);
            _gameStateMachine = gameStateMachine;
        }

        public void Enter(string sceneName)
        {
            _gameInitializer.Curtain.Show();
            _gameInitializer.GameFactory.CleanUp();
            _spawnManager = _gameInitializer.GameFactory.CreateSpawnManager(sceneName);
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _gameInitializer.Curtain.Hide();
        }

        private void OnLoaded()
        {
            string levelToLoad = _gameInitializer.ProgressService.Progress.lastState.levelToLoad;

            if (enterCheck == 0)
            {
                enterCheck++;
                InitGameWorld(levelToLoad);
                InformProgressReaders();
            }
            

            _gameStateMachine.Enter<GameLoopState, SpawnManager>(_spawnManager);
        }
        
        private void InitGameWorld(string sceneName)
        {
            Player player = _gameInitializer.GameFactory.CreatePlayer();
            _gameInitializer.GameFactory.CreateHud(
                sceneName, 
                player, 
                _gameInitializer.ProgressService);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameInitializer.GameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_gameInitializer.ProgressService.Progress);
            }
        }
    }
}
