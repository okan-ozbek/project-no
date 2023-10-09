using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(PlayerController2D))]
    public sealed class Player : MonoBehaviour
    {
        private float _gravity = -20.0f;

        public float moveSpeed = 6.0f;
        public float jumpHeight = 2.0f;
        public float secondsToJumpApex = 0.8f;

        public float accelerationTimeAirborne = 0.2f;
        public float accelerationTimeGrounded = 0.1f;
        
        [SerializeField] private float upwardForce;
        [SerializeField] private Vector3 velocity;

        private float _velocityXSmoothing;

        private PlayerController2D _playerController2D;
        
        private void Awake()
        {
            _playerController2D = GetComponent<PlayerController2D>();

            _gravity = -(2 * jumpHeight) / Mathf.Pow(secondsToJumpApex, 2);
            upwardForce = Mathf.Abs(_gravity * secondsToJumpApex);

            print($"Gravity: {_gravity}, Upward Force: {upwardForce}");
        }

        private void Start()
        {
        
        }
    
        private void Update()
        {
            // @TODO not having the `&& velocity.y < 0.0f` causes the player to not Jump when on an steeper angled slope.
            if (_playerController2D.CollisionInfo.Above || _playerController2D.CollisionInfo.Below && velocity.y < 0.0f)
            {
                velocity.y = 0.0f;
            }
            
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space) && _playerController2D.CollisionInfo.Below)
            {
                print("Jumped");
                velocity.y = upwardForce;
            }

            float targetVelocityX = input.x * moveSpeed;
            float accelerationTime = (_playerController2D.CollisionInfo.Below) 
                ? accelerationTimeGrounded
                : accelerationTimeAirborne;
                
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref _velocityXSmoothing, accelerationTime);
            
            velocity.x = input.x * moveSpeed;
            velocity.y += _gravity * Time.deltaTime;

            _playerController2D.Move(velocity * Time.deltaTime);
        }
    }
}
