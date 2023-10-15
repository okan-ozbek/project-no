namespace Player.States
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerController controller) : base(PlayerStateEnum.Jump, controller)
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
            return PlayerStateEnum.Jump;
        }
    }
}