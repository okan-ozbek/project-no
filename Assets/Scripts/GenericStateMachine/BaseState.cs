using System;

namespace GenericStateMachine
{
    public abstract class BaseState<TState> where TState : Enum
    {
        public TState StateKey { get; }

        protected BaseState(TState stateKey)
        {
            StateKey = stateKey;
        }

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnLeave();

        public abstract TState GetNextState();
    }    
}

