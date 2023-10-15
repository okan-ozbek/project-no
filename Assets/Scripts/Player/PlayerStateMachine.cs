using System.Collections.Generic;
using GenericStateMachine;
using Player.States;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public sealed class PlayerStateMachine : StateManager<PlayerStateEnum>
    {
        private PlayerController _controller;
        
        protected override void Start()
        {
            _controller = GetComponent<PlayerController>();

            SetStates();
            
            CurrentState = States[PlayerStateEnum.Fall];
            
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            
            print(CurrentState);
        }

        private void SetStates()
        {
            States = new Dictionary<PlayerStateEnum, BaseState<PlayerStateEnum>>
            {
                { PlayerStateEnum.Idle, new PlayerIdleState(_controller) },
                { PlayerStateEnum.Fall, new PlayerFallState(_controller) },
                { PlayerStateEnum.Jump, new PlayerJumpState(_controller) },
                { PlayerStateEnum.Run, new PlayerRunState(_controller) }
            };
        }
    }
}