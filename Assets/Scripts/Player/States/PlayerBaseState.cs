using GenericStateMachine;

namespace Player.States
{
    public abstract class PlayerBaseState : BaseState<PlayerStateEnum>
    {
        protected readonly PlayerController Controller;
        protected readonly PlayerStateSwitch StateSwitch;

        protected PlayerBaseState(PlayerStateEnum stateKey, PlayerController controller) : base(stateKey)
        {
            Controller = controller;

            StateSwitch = new PlayerStateSwitch(controller);
        }

        public abstract override void OnEnter();
        public abstract override void OnUpdate();
        public abstract override void OnLeave();
        public abstract override PlayerStateEnum GetNextState();
    }
}