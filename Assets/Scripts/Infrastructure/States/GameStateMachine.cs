using System;
using System.Collections.Generic;
using Ammo;
using DefaultNamespace;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Logic;
using MyScreen;
using UnityEngine;
using Zenject;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;
        [Inject] private ISaveLoadService _saveLoadService;
        [Inject] private IPersistentProgressService _progressService;

        public GameStateMachine(
            SceneLoader sceneLoader, 
            LoadingCurtain curtain, 
            AllServices services,
            Camera camera, 
            SpriteRenderer spriteRenderer, 
            BulletContainer bulletParent, 
            CameraShake cameraShake, 
            string initialLevel, 
            int initialPlayerHealth, 
            int initialEnemyHealth, 
            SpawnersWrapper spawnersWrapper, 
            int wave)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(
                    this, 
                    sceneLoader, 
                    services, 
                    camera, 
                    spriteRenderer, 
                    bulletParent, 
                    cameraShake),
                [typeof(LoadLevelState)] = new LoadLevelState(
                    this, 
                    sceneLoader, 
                    curtain, 
                    services.Single<IGameFactory>(), 
                    _progressService),
                [typeof(LoadProgressState)] = new LoadProgressState(
                    this, 
                    _progressService, 
                    _saveLoadService, 
                    initialLevel, 
                    initialPlayerHealth, 
                    initialEnemyHealth, 
                    spawnersWrapper, 
                    wave),
                [typeof(GameLoopState)] = new GameLoopState(this)
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}
