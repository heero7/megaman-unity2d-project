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
        [HideInInspector] protected const float SkinWidth = 0.015f;
        [SerializeField] protected int horizontalRayCount = 4;
        [HideInInspector] protected float horizontalRaySpacing;
        [SerializeField] protected int verticalRayCount = 4;
        [HideInInspector] protected float verticalRaySpacing;
        protected RaycastOrigins raycastOrigins;
        protected BoxCollider2D _collider;
        [SerializeField] protected LayerMask groundLayer;

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
            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
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

