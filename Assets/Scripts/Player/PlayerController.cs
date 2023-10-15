using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PlayerStateMachine))]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float horizontalSpeed;
        public float verticalSpeed;
        public float upforce;
        
        [Header("Collisions")]
        public float groundCollisionCheckRadius;
        public float wallCollisionCheckRadius;
        public LayerMask groundCollisionLayer;
        public LayerMask wallCollisionLayer;
        
        public bool OnGround { get; private set;  }
        public bool OnWall { get; private set;  }
        public float DefaultGravityScale { get; private set; }

        [HideInInspector] public new Rigidbody2D rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            DefaultGravityScale = rigidbody.gravityScale;
        }

        private void Update()
        {
            // TODO Add better movement
            rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * horizontalSpeed * Time.deltaTime, rigidbody.velocity.y);
        }
        
        private void FixedUpdate()
        {
            OnGround = CheckOnGround();
            OnWall = CheckOnWall();
        }

        public void SetGravityScale(float newGravityScale)
        {
            rigidbody.gravityScale = newGravityScale;
        }
        
        private bool CheckOnGround()
        {
            Vector3 groundCheckPosition = transform.position;
            groundCheckPosition.y -= transform.localScale.y * 0.5f;

            return Physics2D.OverlapCircle(groundCheckPosition, groundCollisionCheckRadius, groundCollisionLayer);
        }

        private bool CheckOnWall()
        {
            return false;
        }
    }
}