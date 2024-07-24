using System;
using System.Collections.Generic;
using Infrastructure.GameLaunch;
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
            GameInitializer gameInitializer, 
            ICoroutineRunner coroutineRunner,
            SignalBus signalBus)
        {
            SceneLoader sceneLoader = new(coroutineRunner);
            
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(sceneLoader, this),
                [typeof(LoadProgressState)] = new LoadProgressState(gameInitializer, this),
                [typeof(LoadLevelState)] = new LoadLevelState(gameInitializer, coroutineRunner, this),
                [typeof(GameLoopState)] = new GameLoopState(this, gameInitializer, signalBus)
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
        
        public void Enter<TState, TPayload1, TPayLoad2, TPayload3>(
            TPayload1 payload1, 
            TPayLoad2 payLoad2, 
            TPayload3 payload3) where TState : class, IPayloadedState2<TPayload1, TPayLoad2, TPayload3>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload1, payLoad2, payload3);
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
