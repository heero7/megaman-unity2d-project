using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RaycastPhysics
{
    public class MovingPlatformController : RaycastController
    {
        [SerializeField] private Vector2 move;
        [SerializeField] private LayerMask passengerLayer; // Those who will be passengers on the moving platform.

        private List<PassengerMovement> calculatedPassengerMovment;
        private Dictionary<Transform, Controller2D> passengerCache = new Dictionary<Transform, Controller2D>();


        private void Update()
        {
            UpdateRayCastOrigins();

            var velocity = move * Time.deltaTime;
            CalculatePassengerMovement(velocity);
            
            MovePassengers(true);
            transform.Translate(velocity);
            MovePassengers(false);
        }

        /// <summary>
        /// When moving up or down and looking for collisions,
        /// we need to either move the platform first,
        /// or the passenger first.
        /// </summary>
        /// <param name="beforeMovePlatform">Was this done before calculating the platform movement?</param>
        private void MovePassengers(bool beforeMovePlatform)
        {
            calculatedPassengerMovment?.ForEach(passenger => 
            {
                if (!passengerCache.ContainsKey(passenger.Transform))
                {
                    passengerCache.Add(passenger.Transform, passenger.Transform.GetComponent<Controller2D>());
                }

                if (passenger.MoveBeforePlatform == beforeMovePlatform)
                {
                    passengerCache[passenger.Transform].Move(passenger.Velocity, passenger.StandingOnPlatform);
                }
            });
        }

        /// <summary>
        /// Moves all the objects of this type, that are on
        /// the platform.
        /// </summary>
        /// <param name="velocity"></param>
        private void CalculatePassengerMovement(Vector2 velocity)
        {
            // Store the passengers moved THIS frame
            HashSet<Transform> movedPassengers = new HashSet<Transform>(); // HashSets are fast!
            calculatedPassengerMovment = new List<PassengerMovement>();
            float directionX = Mathf.Sign(velocity.x);
            float directionY = Mathf.Sign(velocity.y);

            #region Vertically moving platform
            if (velocity.y != 0)
            {
                var rayLength = Mathf.Abs(velocity.y) + SkinWidth;

                // Start from bottom left corner
                for (int i = 0; i < verticalRayCount; i++)
                {
                    // If moving up, start from the topleft corner. If moving down, start from bottomleft corner.
                    Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft;
                    rayOrigin += Vector2.right * (verticalRaySpacing * i); // Add velocity on x axis, so we can check to where we will be. (This should only be done when we have a velocity.x value [right?])
                    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerLayer);
                    // Visualize the rays.
                    Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.green);

                    // Found a passenger
                    if (hit)
                    {
                        if (!movedPassengers.Contains(hit.transform))
                        {
                            movedPassengers.Add(hit.transform);
                            // Move the passenger
                            // Let the gap between passenger and platform be 0.
                            float pushX = (directionY == 1) ? velocity.x : 0;
                            float pushY = velocity.y - (hit.distance - SkinWidth) * directionY;
                            calculatedPassengerMovment.Add(
                                new PassengerMovement
                                {
                                    Transform = hit.transform,
                                    Velocity = new Vector2(pushX, pushY),
                                    StandingOnPlatform = directionY == 1,
                                    MoveBeforePlatform = true,
                                }
                            );
                        }
                    }
                }
            }
            #endregion

            #region Horizontally moving platform
            if (velocity.x != 0)
            {
                var rayLength = Mathf.Abs(velocity.x) + SkinWidth;

                // Start from bottom left corner
                for (int i = 0; i < horizontalRayCount; i++)
                {
                    Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
                    rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerLayer);

                    // Found a passenger
                    if (hit)
                    {
                        if (!movedPassengers.Contains(hit.transform))
                        {
                            movedPassengers.Add(hit.transform);
                            // Move the passenger
                            // Let the gap between passenger and platform be 0.
                            float pushX = velocity.x - (hit.distance - SkinWidth) * directionX;
                            float pushY = -SkinWidth; // So the passenger realizes its on the ground, a small force will do that.

                            calculatedPassengerMovment.Add(
                                new PassengerMovement
                                {
                                    Transform = hit.transform,
                                    Velocity = new Vector2(pushX, pushY),
                                    StandingOnPlatform = false,
                                    MoveBeforePlatform = true,
                                }
                            );
                        }
                    }
                }
            }
            #endregion

            #region Passenger is on top of a horizontally or downward moving platform.
            var platformIsMovingDown = directionY == -1;
            var platformIsMovingOnlyHorizontally = velocity.y == 0 && velocity.x != 0;

            if (platformIsMovingDown || platformIsMovingOnlyHorizontally)
            {
                var rayLength = SkinWidth * 2;
                for (int i = 0; i < verticalRayCount; i++)
                {
                    // If moving up, start from the topleft corner. If moving down, start from bottomleft corner.
                    Vector2 rayOrigin = raycastOrigins.TopLeft + Vector2.right * (verticalRaySpacing * i); // Add velocity on x axis, so we can check to where we will be. (This should only be done when we have a velocity.x value [right?])
                    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerLayer);
                    // Visualize the rays.
                    Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.green);

                    // Found a passenger
                    if (hit)
                    {
                        if (!movedPassengers.Contains(hit.transform))
                        {
                            movedPassengers.Add(hit.transform);
                            // Move the passenger
                            // Let the gap between passenger and platform be 0.
                            float pushX = velocity.x;
                            float pushY = velocity.y;

                            calculatedPassengerMovment.Add(
                                new PassengerMovement
                                {
                                    Transform = hit.transform,
                                    Velocity = new Vector2(pushX, pushY),
                                    StandingOnPlatform = true,
                                    MoveBeforePlatform = false,
                                }
                            );
                        }
                    }
                }
            }
            #endregion
        }
    }
}
