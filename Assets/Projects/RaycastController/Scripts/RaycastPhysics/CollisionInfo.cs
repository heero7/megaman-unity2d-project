using UnityEngine;

namespace RaycastPhysics
{
    internal struct CollisionInfo
    {
        public bool Above { get; internal set; }
        public bool Below { get; internal set; }
        public bool Left { get; internal set; }
        public bool Right { get; internal set; }
        public bool ClimbingSlope { get; internal set; }
        public bool DescendingSlope { get; internal set; }
        public float SlopeAngle { get; internal set; }
        public float SlopeAngleOld { get; internal set; }
        public Vector2 VelocityOld { get; internal set; }

        public void Reset()
        {
            Above = Below = false;
            Left = Right = false;
            ClimbingSlope = false;
            DescendingSlope = false;

            SlopeAngleOld = SlopeAngle;
            SlopeAngle = 0;
        }
    }
}