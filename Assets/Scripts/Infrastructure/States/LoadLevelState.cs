using EnemyScripts;
using Infrastructure.Factory;
using Infrastructure.GameLaunch;
using Infrastructure.Signals;
using Interfaces;
using Logic;
using PlayerScripts;
using Zenject;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IPersistentProgressService _progressService;
        private readonly SceneLoader _sceneLoader;
        private readonly SignalBus _signalBus;

        private readonly GameInitializer _gameInitializer;
        
        private SpawnManager _spawnManager;

        public LoadLevelState(GameInitializer gameInitializer, ICoroutineRunner coroutineRunner, SignalBus signalBus)
        {
            _gameInitializer = gameInitializer;
            _sceneLoader = new SceneLoader(coroutineRunner);
            _signalBus = signalBus;
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
            _signalBus.Fire(new LevelLoadLoaded(_spawnManager));
        }
    }
}
