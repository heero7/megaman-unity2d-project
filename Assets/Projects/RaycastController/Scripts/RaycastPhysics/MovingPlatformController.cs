using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RaycastPhysics
{
    public class MovingPlatformController : RaycastController
    {
        [SerializeField] private LayerMask passengerLayer; // Those who will be passengers on the moving platform.
        [SerializeField] private bool isCyclic;
        [SerializeField] private float platformSpeed; // TODO: This should be a FloatReference.
        [Range(0, 2)] [SerializeField] private float easeAmount;
        [SerializeField] private float waitTime; // TODO: This should be a FloatReference.
        [SerializeField] private Vector2[] localWayPoints; // Vector2's relative to the platform.

        private Vector2[] globalWayPoints; // The copy of localWayPoints, in world space.
        private int fromWayPointIndex;
        private float percentBetweenWayPoints; // Percent between 0 - 1.
        private float nextMoveTime;

        private List<PassengerMovement> calculatedPassengerMovment;
        private Dictionary<Transform, Controller2D> passengerCache = new Dictionary<Transform, Controller2D>();

        protected override void Start()
        {
            base.Start();
            globalWayPoints = new Vector2[localWayPoints.Length]; // Initialize to the size.

            // Initialize global way points.
            for (int i = 0; i < localWayPoints.Length; i++)
            {
                globalWayPoints[i] = localWayPoints[i] + (Vector2)transform.position; // Add the position from the start of the game.
            }
        }

        private Vector2 CalculatePlatformMovement()
        {
            if (Time.time < nextMoveTime)
            {
                return Vector2.zero; // Don't move yet.
            }
            fromWayPointIndex %= globalWayPoints.Length; // Resets to 0, if it reaches globalWayPoints.Length
            var toWayPointIndex = (fromWayPointIndex + 1) % globalWayPoints.Length;
            var distanceBetweenWayPoints = Vector2.Distance(globalWayPoints[fromWayPointIndex], globalWayPoints[toWayPointIndex]);
            percentBetweenWayPoints += Time.deltaTime * platformSpeed / distanceBetweenWayPoints; // Further waypoints would cause a faster speed, divide by distance to get a constant rate.
            percentBetweenWayPoints = Mathf.Clamp01(percentBetweenWayPoints); // Clamp between 0 & 1.
            var easePercentBetweenWayPoints = Ease(percentBetweenWayPoints);
            Vector2 nextPosition = Vector2.Lerp(globalWayPoints[fromWayPointIndex], globalWayPoints[toWayPointIndex], easePercentBetweenWayPoints);

            if (percentBetweenWayPoints >= 1) // Platform has reached a waypoint.
            {
                percentBetweenWayPoints = 0; // Reset waypoints.
                fromWayPointIndex++; // Next set of waypoints.
                if (!isCyclic)
                {
                    if (fromWayPointIndex >= globalWayPoints.Length - 1)
                    {
                        fromWayPointIndex = 0;
                        System.Array.Reverse(globalWayPoints);
                    }
                }
                nextMoveTime = waitTime + Time.time;
            }
            return nextPosition - (Vector2)transform.position;
        }

        private void Update()
        {
            UpdateRayCastOrigins();

            var velocity = CalculatePlatformMovement();
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
                    if (hit && hit.distance != 0)
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
                    if (hit && hit.distance != 0)
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
                    if (hit && hit.distance != 0)
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

        private float Ease(float x)
        {
            float a = easeAmount + 1;
            return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (localWayPoints != null)
            {
                var lineLength = 0.3f;
                for (int i = 0; i < localWayPoints.Length; i++)
                {
                    var globalWayPointPosition =
                    Application.isPlaying
                        ? globalWayPoints[i]
                        : localWayPoints[i] + (Vector2)transform.position;
                    Gizmos.DrawLine(globalWayPointPosition - Vector2.up * lineLength, globalWayPointPosition + Vector2.up * lineLength);
                    Gizmos.DrawLine(globalWayPointPosition - Vector2.left * lineLength, globalWayPointPosition + Vector2.left * lineLength);
                }
            }
        }
    }
}
