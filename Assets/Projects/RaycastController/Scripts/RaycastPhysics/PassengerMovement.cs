using UnityEngine;

namespace RaycastPhysics
{
    internal struct PassengerMovement
    {
        public Transform Transform { get; internal set; }
        public Vector2 Velocity { get; internal set; }
        public bool StandingOnPlatform { get; internal set; }
        public bool MoveBeforePlatform { get; internal set; }

        public PassengerMovement(Transform transform, Vector2 velocity, bool standingOnPlatform, bool moveBeforePlatform)
        {
            this.Transform = transform;
            this.Velocity = velocity;
            this.StandingOnPlatform = standingOnPlatform;
            this.MoveBeforePlatform = moveBeforePlatform;
        }
    }
}