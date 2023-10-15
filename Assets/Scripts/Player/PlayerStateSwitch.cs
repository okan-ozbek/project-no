using UnityEngine;

namespace Player
{
    public sealed class PlayerStateSwitch
    {
        private readonly PlayerController _controller;
        
        public PlayerStateSwitch(PlayerController controller)
        {
            _controller = controller;
        }

        // FALL
        public bool FromFallToRun => (_controller.OnGround && Moving());
        public bool FromFallToIdle => (_controller.OnGround && !Moving());

        // IDLE
        public bool FromIdleToRun => (Moving());
        public bool FromIdleToFall => (Falling());

        // RUN
        public bool FromRunToIdle => (!Moving());
        public bool FromRunToFall => (Falling());

        private bool Moving()
        {
            return (Mathf.Abs(_controller.rigidbody.velocity.x) > 0.0f);
        }

        private bool Falling()
        {
            return (_controller.rigidbody.velocity.y < 0.0f);
        }
    }
}