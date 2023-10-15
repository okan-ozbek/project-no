namespace Player.States
{
    public class PlayerRunState : PlayerBaseState
    {
        public PlayerRunState(PlayerController controller) : base(PlayerStateEnum.Run, controller)
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
            if (StateSwitch.FromRunToIdle)
            {
                return PlayerStateEnum.Idle;
            }

            if (StateSwitch.FromRunToFall)
            {
                return PlayerStateEnum.Fall;
            }

            return PlayerStateEnum.Run;
        }
    }
}