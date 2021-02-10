using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaycastPhysics
{
    /// <summary>
    /// RaycastController class, where collision is detected
    /// using raycasts.
    /// NOTE: Auto Sync Transforms MUST BE TURNED ON.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaycastController : MonoBehaviour
    {
        private const float DistanceBetweenRays = 0.25f;
        [HideInInspector] protected const float SkinWidth = 0.015f;
        [HideInInspector] protected int horizontalRayCount;
        [HideInInspector] protected float horizontalRaySpacing;
        [HideInInspector] protected int verticalRayCount;
        [HideInInspector] protected float verticalRaySpacing;
        internal RaycastOrigins raycastOrigins;
        protected BoxCollider2D _collider;
        [SerializeField] protected LayerMask collisionLayer; // Set a default here.

        // Start is called before the first frame update
        protected virtual void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }

        protected Bounds GetColliderBounds()
        {
            var bounds = _collider.bounds;
            bounds.Expand(-SkinWidth * 2);
            return bounds;
        }

        protected void CalculateRaySpacing()
        {
            var bounds = GetColliderBounds();
            var boundsWidth = bounds.size.x;
            var boundsHeight = bounds.size.y;

            horizontalRayCount = Mathf.RoundToInt(boundsHeight / DistanceBetweenRays);
            verticalRayCount = Mathf.RoundToInt(boundsWidth / DistanceBetweenRays);

            horizontalRaySpacing = boundsHeight / (horizontalRayCount - 1);
            verticalRaySpacing = boundsWidth / (verticalRayCount - 1);
        }

        protected void UpdateRayCastOrigins()
        {
            var bounds = GetColliderBounds();

            raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        }
    }
}

