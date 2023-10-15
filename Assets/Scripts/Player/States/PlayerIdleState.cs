namespace Player.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerController controller) : base(PlayerStateEnum.Idle, controller)
        {
            
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnLeave()
        {
            
        }

        public override PlayerStateEnum GetNextState()
        {
            if (StateSwitch.FromIdleToRun)
            {
                return PlayerStateEnum.Run;
            }

            if (StateSwitch.FromIdleToFall)
            {
                return PlayerStateEnum.Fall;
            }
            
            return PlayerStateEnum.Idle;
        }
    }
}