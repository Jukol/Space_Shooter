using System;
using System.Collections.Generic;
using Infrastructure.GameLaunch;
using Interfaces;
using Logic;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(
            GameInitializer gameInitializer, 
            ICoroutineRunner coroutineRunner)
        {
            SceneLoader sceneLoader = new(coroutineRunner);
            
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(sceneLoader, this),
                [typeof(LoadProgressState)] = new LoadProgressState(gameInitializer, this),
                [typeof(LoadLevelState)] = new LoadLevelState(gameInitializer, coroutineRunner, this),
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
