using System;
using Player.Structs;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class PlayerController2D : MonoBehaviour
    {
        private const float SkinWidth = 0.015f;
        
        public LayerMask collisionLayerMask;
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;
        public float maxClimbAngle = 60.0f;
        public float maxDescendAngle = 60.0f;
        
        public CollisionInfo CollisionInfo;
        
        private float _horizontalRaySpacing;
        private float _verticalRaySpacing;
        
        private BoxCollider2D _boxCollider2D;
        private RaycastOrigins _raycastOrigins;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            
            CalculateRaySpacing();
        }

        public void Move(Vector3 velocity)
        {
            UpdateRaycastOrigins();
            CollisionInfo.Reset();
            CollisionInfo.velocityOld = velocity;

            if (velocity.y < 0.0f)
            {
                DescendSlope(ref velocity);
            }
            
            if (velocity.x != 0.0f)
            {
                HorizontalCollisions(ref velocity);    
            }

            if (velocity.y != 0.0f)
            {
                VerticalCollisions(ref velocity);
            }

            transform.Translate(velocity);
        }
        
        private void HorizontalCollisions(ref Vector3 velocity)
        {
            float directionX = Mathf.Sign(velocity.x);
            float rayLength = Mathf.Abs(velocity.x) + SkinWidth;
            
            for (int verticalRay = 0; verticalRay < horizontalRayCount; verticalRay++)
            {
                Vector2 rayOrigin = (directionX == -1.0f)
                    ? _raycastOrigins.BottomLeft
                    : _raycastOrigins.BottomRight;

                rayOrigin += Vector2.up * (_horizontalRaySpacing * verticalRay);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayerMask);
                    
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                    if (verticalRay == 0.0 && slopeAngle <= maxClimbAngle)
                    {
                        float distanceToSlopeStart = 0.0f;

                        if (CollisionInfo.DescendingSlope)
                        {
                            CollisionInfo.DescendingSlope = false;
                            velocity = CollisionInfo.velocityOld;
                        }
                        
                        if (slopeAngle != CollisionInfo.SlopeAngleOld)
                        {
                            distanceToSlopeStart = hit.distance - SkinWidth;
                            velocity.x -= distanceToSlopeStart * directionX;
                        }
                        
                        ClimbSlope(ref velocity, slopeAngle);
                        velocity.x += distanceToSlopeStart * directionX;
                    }

                    if (!CollisionInfo.ClimbingSlope || slopeAngle > maxClimbAngle)
                    {
                        velocity.x = (hit.distance - SkinWidth) * directionX;
                        rayLength = hit.distance;

                        if (CollisionInfo.ClimbingSlope)
                        {
                            velocity.y = Mathf.Tan(CollisionInfo.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                        }

                        CollisionInfo.Left = (directionX == -1.0f);
                        CollisionInfo.Right = (directionX == 1.0f);
                    }
                }
            }
        }

        private void ClimbSlope(ref Vector3 velocity, float slopeAngle)
        {
            float moveDistance = Mathf.Abs(velocity.x);
            float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
            
            print(climbVelocityY);

            if (velocity.y <= climbVelocityY)
            {
                velocity.y = climbVelocityY;
                velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            }
            
            CollisionInfo.Below = true;
            CollisionInfo.ClimbingSlope = true;
            CollisionInfo.SlopeAngle = slopeAngle;
        }

        private void DescendSlope(ref Vector3 velocity)
        {
            float directionX = Mathf.Sign(velocity.x);
            Vector2 rayOrigin = (directionX == -1)
                ? _raycastOrigins.BottomRight
                : _raycastOrigins.BottomLeft;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionLayerMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - SkinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                        {
                            float moveDistance = Mathf.Abs(velocity.x);
                            float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            
                            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                            velocity.y -= descendVelocityY;

                            CollisionInfo.SlopeAngle = slopeAngle;
                            CollisionInfo.DescendingSlope = true;
                            CollisionInfo.Below = true;
                        }
                    }
                }
            }
        }
        
        private void VerticalCollisions(ref Vector3 velocity)
        {
            float directionY = Mathf.Sign(velocity.y);
            float rayLength = Mathf.Abs(velocity.y) + SkinWidth;
            
            for (int verticalRay = 0; verticalRay < verticalRayCount; verticalRay++)
            {
                Vector2 rayOrigin = (directionY == -1.0f)
                    ? _raycastOrigins.BottomLeft
                    : _raycastOrigins.TopLeft;

                rayOrigin += Vector2.right * (_verticalRaySpacing * verticalRay + velocity.x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayerMask);
                    
                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if (hit)
                {
                    velocity.y = (hit.distance - SkinWidth) * directionY;
                    rayLength = hit.distance;

                    if (CollisionInfo.ClimbingSlope)
                    {
                        velocity.x = velocity.y / Mathf.Tan(CollisionInfo.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    }
                    
                    CollisionInfo.Below = (directionY == -1.0f);
                    CollisionInfo.Above = (directionY == 1.0f);
                }
            }

            if (CollisionInfo.ClimbingSlope)
            {
                float directionX = Mathf.Sign(velocity.x);
                rayLength = Mathf.Abs(velocity.x) + SkinWidth;
                Vector2 rayOrigin = (directionX == -1)
                    ? _raycastOrigins.BottomLeft
                    : _raycastOrigins.BottomRight;

                rayOrigin  += Vector2.up * velocity.y;

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayerMask);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                    if (slopeAngle != CollisionInfo.SlopeAngle)
                    {
                        velocity.x = (hit.distance - SkinWidth) * directionX;
                        CollisionInfo.SlopeAngle = slopeAngle;
                    }
                }
            }
        }

        private void UpdateRaycastOrigins()
        {
            Bounds bounds = _boxCollider2D.bounds;
            bounds.Expand((SkinWidth * -2.0f));
            
            _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            _raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            _raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        private void CalculateRaySpacing()
        {
            Bounds bounds = _boxCollider2D.bounds;
            bounds.Expand((SkinWidth * -2.0f));
            
            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            _horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            _verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }
    }
}