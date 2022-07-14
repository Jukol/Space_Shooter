﻿using Infrastructure.Factory;
using Logic;
namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        
        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
        }
        
        public void Enter(string sceneName)
        {
            _curtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }
        
        public void Exit() => 
            _curtain.Hide();
        
        private void OnLoaded()
        {
            _gameFactory.CreatePlayer();
            _gameFactory.CreateSpawnManager();
            _gameFactory.CreateHud();
            
            _stateMachine.Enter<GameLoopState>();
        }
    }
}
