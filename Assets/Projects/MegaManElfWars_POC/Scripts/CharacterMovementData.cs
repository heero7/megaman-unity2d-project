using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterMovementData/MovementData")]
public class CharacterMovementData : ScriptableObject
{
    public new string name;
    public int health = 10;
    public float jumpHeight = 5f;
    public float timeToJumpApex = 2f;
    public float runSpeed = 4f;
    public float dashDuration = 2f;
    public float wallSlideSpeed = 2f;
    public float wallJumpVelocity = 10f;
    public Vector2 WallJumpAngle = new Vector2(2, 4);
    public float wallClimbNoControlTimer = 0.1f;
    public float ladderClimbSpeed = 2f;
    [Header("Dash Properties")]
    public float dashSpeed = 6f;
    public float DashDuration = 0.5f;
    public float DashCoolDown = 1f;
    [Range(1f,2f)] public float DashJumpAerialSpeed = 1.5f;
    public float hurtTime = 1f;
}
