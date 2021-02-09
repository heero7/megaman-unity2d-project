using UnityEngine;

[CreateAssetMenu(fileName = "JumpData", menuName = "MovmentData/JumpData", order = 0)]
public class JumpData : ScriptableObject
{
    public FloatReference jumpHeight;
    public FloatReference timeToJumpApex;
}