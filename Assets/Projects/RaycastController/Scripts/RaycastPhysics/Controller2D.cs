﻿using UnityEngine;

namespace RaycastPhysics
{
    /// <summary>
    /// Controller2D class, place this on objects you want
    /// to control.
    /// NOTE: This expects a certain tag name for multi-way platforms, change them as needed. 
    /// </summary>
    public class Controller2D : RaycastController
    {
        private const string MultiWayPlatform = "MultiWayPlatform";
        private const float MaxClimbAngle = 80f;
        private const float MaxDescendAngle = 75f;
        private CollisionInfo collisionInfo;
        internal CollisionInfo CollisionInfo { get => collisionInfo; }
        private Vector2 playerInput;

        protected override void Start() // TODO: Can this just be an OnEnable?
        {
            base.Start();
            collisionInfo.FacingDirection = 1;
        }

        /// <summary>
        /// Mutates the veocity so that the controller does not collide with other objects.
        /// After the collision is handled, the gameobject is translated by the resulting
        /// velocity.
        /// </summary>
        /// <param name="deltaMove">Requested speed.</param>
        /// <param name="standingOnPlatform">Is the gameobject currently standing on a platform.</param>
        public void Move(Vector2 deltaMove, bool standingOnPlatform)
        {
            Move(deltaMove, Vector2.zero, standingOnPlatform);
        }

        /// <summary>
        /// Mutates the veocity so that the controller does not collide with other objects.
        /// After the collision is handled, the gameobject is translated by the resulting
        /// velocity.
        /// </summary>
        /// <param name="deltaMove">Requested speed.</param>
        /// <param name="input">Requested input (for multiway platforms)</param>
        /// <param name="standingOnPlatform">Is the gameobject currently standing on a platform.</param>
        public void Move(Vector2 deltaMove, Vector2 input, bool standingOnPlatform = false)
        {
            playerInput = input;
            // Every frame we have to update the ray origins
            UpdateRayCastOrigins();
            // Reset the collisions
            collisionInfo.Reset();
            collisionInfo.VelocityOld = deltaMove; // Store the velocity before it is  changes

            if (deltaMove.x != 0)
            {
                collisionInfo.FacingDirection = (int)Mathf.Sign(deltaMove.x);
            }

            if (deltaMove.y < 0) DescendSlope(ref deltaMove); // Descend on a slope
            // Handle Collisions.
            HandleHorizontalCollisions(ref deltaMove);
            if (deltaMove.y != 0) HandleVerticalCollisions(ref deltaMove);

            transform.Translate(deltaMove);

            // If standing on a platform, allow for jumping.
            if (standingOnPlatform) collisionInfo.Below = true;
        }

        private void HandleVerticalCollisions(ref Vector2 velocity)
        {
            var directionY = Mathf.Sign(velocity.y); // Direction of the y velocity. (Sign only returns 1 OR -1)
            var rayLength = Mathf.Abs(velocity.y) + SkinWidth;

            // Start from bottom left corner
            for (int i = 0; i < verticalRayCount; i++)
            {
                // If moving up, start from the topleft corner. If moving down, start from bottomleft corner.
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x); // Add velocity on x axis, so we can check to where we will be. (This should only be done when we have a velocity.x value [right?])
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayer);

                // Visualize the rays.
                Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.green);

                if (hit)
                {
                    if (hit.collider.CompareTag(MultiWayPlatform)) // Ignore everything, if this is a MultiWay platform.
                    {
                        if (directionY == 1 || hit.distance == 0) continue; // The zero case prevents ontroller from making it even though it didn't clear the platform.
                        if (collisionInfo.FallingThroughPlatform) continue;
                        if (playerInput.y == -1) // The input handler requested to fall through the bottom of this platform.
                        {
                            collisionInfo.FallingThroughPlatform = true;
                            Invoke("ResetFallingThroughPlatformFlag", 0.5f);
                            continue;
                        }
                    }
                    // Raycast has hit the ground layer.
                    // 1. Set Y velocity equal to the amount the object must move from the current position, to the point where the ray intersects.
                    velocity.y = (hit.distance - SkinWidth) * directionY; // maintain direction by multiplying directionY.
                    rayLength = hit.distance; // If we have found something, change the raylength to the distance, so we dont' go through things.

                    if (collisionInfo.ClimbingSlope) // We're losing the x velocity needed in subsequent iterations, when we collide with an object.
                    {
                        velocity.x = velocity.y / Mathf.Tan(collisionInfo.SlopeAngle * Mathf.Deg2Rad);
                    }

                    collisionInfo.Below = directionY == -1;
                    collisionInfo.Above = directionY == 1;
                }
            }

            if (collisionInfo.ClimbingSlope) // Case for curved slopes.
            {
                float directionX = Mathf.Sign(velocity.x);
                rayLength = Mathf.Abs(velocity.x) + SkinWidth;
                Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight) + Vector2.up * velocity.y;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayer);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (slopeAngle != collisionInfo.SlopeAngle)
                    {
                        velocity.x = (hit.distance - SkinWidth) * directionX;
                        collisionInfo.SlopeAngle = slopeAngle;
                    }
                }
            }
        }

        private void HandleHorizontalCollisions(ref Vector2 velocity)
        {
            var directionX = collisionInfo.FacingDirection; // Direction of the x velocity. (Sign only returns 1 OR -1)
            var rayLength = Mathf.Abs(velocity.x) + SkinWidth;

            if (Mathf.Abs(velocity.x) < SkinWidth)
            {
                rayLength = 2 * SkinWidth; // Send a small ray to check if touching the wall.
            }

            // Start from bottom left corner
            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayer);

                // Visualize the rays.
                Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

                if (hit)
                {
                    // If we're colliding with a moving platform that has moved through this object.
                    if (hit.distance == 0) continue;
                    // To climb slopes.
                    // Find the angle between the normal and Vector2.up, this is the angle of the slopoe we're trying to climb.
                    // The ray hit the slope, at the point where we're trying to go.
                    var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (i == 0 && slopeAngle <= MaxClimbAngle) // Only check at the bottom most ray and only if the angle isn't too steep.
                    {
                        if (collisionInfo.DescendingSlope)
                        {
                            // We're not actually descending a slope.
                            collisionInfo.DescendingSlope = false;
                            velocity = collisionInfo.VelocityOld; // Return the original velocity.
                        }
                        // Because when we detect that we'll climb a slope, we'll be farther than the actual point where the slope actually is
                        //
                        float distanceToSlopeStart = 0;
                        if (slopeAngle != collisionInfo.SlopeAngleOld) // Starting to climb a new slope.
                        {
                            distanceToSlopeStart = hit.distance - SkinWidth;
                            velocity.x -= distanceToSlopeStart * directionX;
                        }
                        ClimbSlope(ref velocity, slopeAngle);
                        velocity.x += distanceToSlopeStart * directionX;
                    }

                    // Only continue if we're not climbing the slope. Since we only want to run ClimbSlope() once.
                    if (!collisionInfo.ClimbingSlope || slopeAngle > MaxClimbAngle)
                    {
                        // Raycast has hit the ground layer.
                        /** 
                            NOTE: Could solve double slope problem
                            Found in comments from Episode 4.<!-- Sebastian Lague Tutorial -->
                            velocity.x = Mathf.Min(Mathf.Abs(velocity.x), (hit.distance - skinWidth)) * directionX;
                            rayLength = Mathf.Min(Mathf.Abs(velocity.x) + skinWidth, hit.distance);
                        */
                        // 1. Set Y velocity equal to the amount the object must move from the current position, to the point where the ray intersects.
                        velocity.x = (hit.distance - SkinWidth) * directionX; // maintain direction by multiplying directionY.
                        rayLength = hit.distance; // If we have found something, change the raylength to the distance, so we dont' go through things.

                        if (collisionInfo.ClimbingSlope) // We're losing the y velocity needed in subsequent iterations, when we collide with an object.
                        {
                            velocity.y = Mathf.Tan(collisionInfo.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                        }

                        collisionInfo.Left = directionX == -1;
                        collisionInfo.Right = directionX == 1;
                    }
                }
            }
        }

        private void ClimbSlope(ref Vector2 velocity, float slopeAngle)
        {
            // Set speed to the same as normal movespeed.
            // Treat velocity.x is the total distance we want to move.
            // Use target velocityX to find new velocity x AND y should be 
            // y = distance * sin(theta) 
            // x = distance * cos(theta)
            // where theta = slopeAngle
            // where distance = velocity.x

            float distance = Mathf.Abs(velocity.x); // Absolute value, so we don't flub calculations.
            float climbVeloictyY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * distance;
            if (velocity.y <= climbVeloictyY)
            {
                velocity.y = climbVeloictyY;
                velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * distance * Mathf.Sign(velocity.x); // Multiply by velocity.x so we head the right direction.
                collisionInfo.Below = true;// This is so we can jump. Because we have a velocity that is not 0, Collision.Below is false in Vertical collisions.
                collisionInfo.ClimbingSlope = true;
                collisionInfo.SlopeAngle = slopeAngle;
            }
        }

        private void DescendSlope(ref Vector2 velocity)
        {
            float directionX = Mathf.Sign(velocity.x);
            // Cast a ray downwards.
            // When going down a slope, the opposite bottom corner is what is touching the slope.
            Vector2 rayOrigin = directionX == -1 ? raycastOrigins.BottomRight : raycastOrigins.BottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionLayer);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= MaxDescendAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX) // Moving down the slope
                    {
                        if (hit.distance - SkinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                        {
                            float distance = Mathf.Abs(velocity.x);
                            float descendVeloictyY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * distance;
                            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * distance * Mathf.Sign(velocity.x); // Multiply by velocity.x so we head the right direction.
                            velocity.y -= descendVeloictyY;

                            collisionInfo.SlopeAngle = slopeAngle;
                            collisionInfo.DescendingSlope = true;
                            collisionInfo.Below = true;
                        }
                    }
                }
            }
        }

        private void ResetFallingThroughPlatformFlag() => collisionInfo.FallingThroughPlatform = false;
    }
}