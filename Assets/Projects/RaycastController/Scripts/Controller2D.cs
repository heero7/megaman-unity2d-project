using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller2D using raycasts.
/// NOTE: Auto Sync Transforms MUST BE TURNED ON.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public partial class Controller2D : MonoBehaviour
{
    private const float SkinWidth = 0.015f;
    private const float MaxClimbAngle = 80f;
    #region Components
    private BoxCollider2D _collider;
    #endregion

    #region Raycasting
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int horizontalRayCount = 4;
    private float horizontalRaySpacing;
    [SerializeField] private int verticalRayCount = 4;
    private float verticalRaySpacing;
    private RaycastOrigins raycastOrigins;
    private CollisionInfo collisionInfo;
    public CollisionInfo CollisionInfo { get => collisionInfo; }
    #endregion

    private Bounds GetColliderBounds()
    {
        var bounds = _collider.bounds;
        bounds.Expand(-SkinWidth * 2);
        return bounds;
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
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, groundLayer);

            // Visualize the rays.
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.green);

            if (hit)
            {
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
        }
    }

    private void HandleHorizontalCollisions(ref Vector2 velocity)
    {
        var directionX = Mathf.Sign(velocity.x); // Direction of the x velocity. (Sign only returns 1 OR -1)
        var rayLength = Mathf.Abs(velocity.x) + SkinWidth;

        // Start from bottom left corner
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, groundLayer);

            // Visualize the rays.
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                // To climb slopes.
                // Find the angle between the normal and Vector2.up, this is the angle of the slopoe we're trying to climb.
                // The ray hit the slope, at the point where we're trying to go.
                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= MaxClimbAngle) // Only check at the bottom most ray and only if the angle isn't too steep.
                {
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

    private void DescendSlope()
    {

    }

    public void Move(Vector2 velocity)
    {
        // Every frame we have to update the ray origins
        UpdateRayCastOrigins();
        // Reset the collisions
        collisionInfo.Reset();
        // Handle Collisions.
        if (velocity.x != 0) HandleHorizontalCollisions(ref velocity);
        if (velocity.y != 0) HandleVerticalCollisions(ref velocity);


        transform.Translate(velocity);
    }

    private void CalculateRaySpacing()
    {
        var bounds = GetColliderBounds();
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    private void UpdateRayCastOrigins()
    {
        var bounds = GetColliderBounds();

        raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }
}
