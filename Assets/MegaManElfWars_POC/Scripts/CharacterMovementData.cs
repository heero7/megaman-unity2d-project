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
    public float dashSpeed = 6f;
    public float dashDuration = 2f;
    public float wallSlideSpeed = 2f;
    public float ladderClimbSpeed = 2f;
}
