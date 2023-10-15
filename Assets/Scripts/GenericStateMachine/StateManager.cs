using System;
using System.Collections.Generic;
using UnityEngine;

namespace GenericStateMachine
{
    public abstract class StateManager<TState> : MonoBehaviour where TState : Enum
    {
        protected Dictionary<TState, BaseState<TState>> States = new();
        protected BaseState<TState> CurrentState;

        protected bool StateTransitionActive = false;

        protected virtual void Start()
        {
            CurrentState.OnEnter();
        }

        protected virtual void Update()
        {
            TState nextStateKey = CurrentState.GetNextState();

            if (StateTransitionActive) return;

            if (nextStateKey.Equals(CurrentState.StateKey))
            {
                CurrentState.OnUpdate();
            }
            else
            {
                TransitionToState(nextStateKey);
            }
        }

        private void TransitionToState(TState stateKey)
        {
            StateTransitionActive = true;
            
            CurrentState.OnLeave();
            CurrentState = States[stateKey];
            CurrentState .OnEnter();

            StateTransitionActive = false;
        }
    }
}
