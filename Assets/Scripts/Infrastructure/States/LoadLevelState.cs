using EnemyScripts;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Logic;
using PlayerScripts;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly SceneLoader _sceneLoader;
        private readonly GameStateMachine _stateMachine;

        private string _sceneName;

        public LoadLevelState(
            GameStateMachine stateMachine, 
            SceneLoader sceneLoader, 
            LoadingCurtain curtain, 
            IGameFactory gameFactory, 
            IPersistentProgressService progressService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
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
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
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
