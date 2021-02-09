public struct CollisionInfo
{
    public bool Above { get; set; }
    public bool Below { get; set; }
    public bool Left { get; set; }
    public bool Right { get; set; }
    public bool ClimbingSlope { get; set; }
    public float SlopeAngle { get; set; }
    public float SlopeAngleOld { get; set; }

    public void Reset()
    {
        Above = Below = false;
        Left = Right = false;
        ClimbingSlope = false;

        SlopeAngleOld = SlopeAngle;
        SlopeAngle = 0;
    }
}

