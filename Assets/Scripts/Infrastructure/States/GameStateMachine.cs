using System;
using System.Collections.Generic;
using Interfaces;
using Logic;
using Zenject;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(
            SceneLoader sceneLoader, 
            LoadingCurtain curtain, 
            string initialLevel, 
            int initialPlayerHealth, 
            SpawnersWrapper spawnersWrapper, 
            int wave,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService,
            IGameFactory gameFactory,
            SignalBus signalBus)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(
                    sceneLoader,
                    signalBus),
                [typeof(LoadLevelState)] = new LoadLevelState(
                    sceneLoader, 
                    curtain, 
                    gameFactory, 
                    progressService,
                    signalBus),
                [typeof(LoadProgressState)] = new LoadProgressState(
                    progressService, 
                    saveLoadService, 
                    initialLevel, 
                    initialPlayerHealth, 
                    spawnersWrapper, 
                    wave,
                    signalBus),
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
