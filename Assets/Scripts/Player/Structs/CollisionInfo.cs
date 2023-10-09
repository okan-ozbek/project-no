using UnityEngine;

namespace Player.Structs
{
    public struct CollisionInfo
    {
        public bool Above, Below, Left, Right;

        public bool ClimbingSlope, DescendingSlope;

        public float SlopeAngle, SlopeAngleOld;

        public Vector3 velocityOld;

        public void Reset()
        {
            (Above, Below, Left, Right, ClimbingSlope, DescendingSlope) = (false, false, false, false, false, false);

            SlopeAngleOld = SlopeAngle;
            SlopeAngle = 0.0f;
        }
    }
}