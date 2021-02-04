using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerMovementState
{
    public Vector2 sendDistance;
    public bool aLotOfDamageTaken;
    private const string lightDamageAnim = "HurtLight";
    private const string heavyDamageAnim = "HurtHeavy";

    public PlayerHurtState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public override void OnEnter()
    {
        if (aLotOfDamageTaken)
        {
            player.Animator.SetBool(heavyDamageAnim, true);
        }
        else
        {
            player.Animator.SetBool(lightDamageAnim, true);
        }
    }

    public override void OnExecute()
    {
        player.StartCoroutine(Stall());
        if (player.IsGrounded())
        {
            movementStateMachine.ChangeState(player.Idle);
        }
        else
        {
            movementStateMachine.ChangeState(player.Falling);
        }
    }

    IEnumerator Stall()
    {
        Debug.Log("Player took damage");
        yield return new WaitForSeconds(player.MovementData.hurtTime);

    }

    public override void OnExit()
    {
        player.Animator.SetBool(heavyDamageAnim, false);
        player.Animator.SetBool(lightDamageAnim, false);
    }

    public override string ToString()
    {
        return "Hurt";
    }
}
