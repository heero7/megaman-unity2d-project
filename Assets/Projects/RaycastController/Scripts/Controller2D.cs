using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    private const float SkinWidth = 0.015f;
    #region Components
    private BoxCollider2D _collider;
    #endregion

    #region Raycasting
    [SerializeField] private int horizontalRayCount = 4;
    private float horizontalRaySpacing;
    [SerializeField] private int verticalRayCount = 4;
    private float verticalRaySpacing;
    private RaycastOrigins _raycastOrigins;
    #endregion

    private Bounds GetColliderBounds()
    {
        var bounds = _collider.bounds;
        bounds.Expand(-SkinWidth * 2);
        return bounds;
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

        _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRayCastOrigins();
        CalculateRaySpacing();
        DrawDebugRays();
    }

    private void DrawDebugRays()
    {
        for (int i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(_raycastOrigins.BottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);
        }
    }
}
