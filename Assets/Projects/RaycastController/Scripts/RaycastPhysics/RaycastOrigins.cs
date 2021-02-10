using UnityEngine;

namespace RaycastPhysics
{
    internal struct RaycastOrigins
    {
        public Vector2 TopLeft { get; set; }
        public Vector2 TopRight { get; set; }
        public Vector2 BottomLeft { get; set; }
        public Vector2 BottomRight { get; set; }
    }
}