using EnemyScripts;
using Infrastructure.Factory;
using Infrastructure.Signals;
using Interfaces;
using Logic;
using PlayerScripts;
using Zenject;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private string _sceneName;
        
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly SceneLoader _sceneLoader;
        private readonly SignalBus _signalBus;

        public LoadLevelState( 
            SceneLoader sceneLoader, 
            LoadingCurtain curtain, 
            IGameFactory gameFactory, 
            IPersistentProgressService progressService,
            SignalBus signalBus)
        {
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _signalBus = signalBus;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.CleanUp();
            _sceneName = sceneName;
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private void OnLoaded()
        {
            // InitGameWorld();
            // InformProgressReaders();
            //
            // _gameFactory.LaunchSpawnManager();
            //
            // _stateMachine.Enter<GameLoopState>();
            _signalBus.Fire<LevelLoadLoaded>();
        }

        private void InitGameWorld()
        {
            Player player = _gameFactory.CreatePlayer();
            SpawnManager spawnManager = _gameFactory.CreateSpawnManager();
            _gameFactory.CreateHud(spawnManager, _sceneName, player, _progressService);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }
    }
}
