using UnityEngine;

namespace Player.States
{
    public sealed class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerController controller) : base(PlayerStateEnum.Fall, controller) { }

        public override void OnEnter()
        {
            Controller.SetGravityScale(Controller.DefaultGravityScale * 2.0f);
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnLeave()
        {
            Controller.SetGravityScale(Controller.DefaultGravityScale);
        }

        public override PlayerStateEnum GetNextState()
        {
            if (StateSwitch.FromFallToIdle)
            {
                return PlayerStateEnum.Idle;
            }

            if (StateSwitch.FromFallToRun)
            {
                return PlayerStateEnum.Run;
            }

            return PlayerStateEnum.Fall;
        }
    }
}