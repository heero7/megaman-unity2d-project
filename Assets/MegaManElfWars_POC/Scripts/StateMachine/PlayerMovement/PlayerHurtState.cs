using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerMovementState
{
    public Vector2 sendDistance;
    public bool aLotOfDamageTaken;
    private float timeInState;
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
        player.SetVelocityY(0);
        player.SetVelocityX(0);
        
        timeInState = 0f;
        once = true;
    }

    bool once;

    public override void OnExecute()
    {
        timeInState += Time.deltaTime;

        if (once) 
        {
            player.Rigidbody2D.AddForce(new Vector2(-player.FacingDirection * 3, 0), ForceMode2D.Impulse);
            once = false;
        }

        if (timeInState >= player.MovementData.hurtTime)
        {
            player.SetVelocityX(0f);
            // Exit.
            if (player.IsGrounded())
            {
                movementStateMachine.ChangeState(player.Idle);
                return;
            }
            else
            {
                movementStateMachine.ChangeState(player.Falling);
                return;
            }
        }
        // Instead of using a coroutine.. wait for an actual amount of synchronous time.
    }

    private bool IsGrounded;

    IEnumerator Stall()
    {
        Debug.Log("Player took damage");
        yield return new WaitForSeconds(player.MovementData.hurtTime);

        if (player.IsGrounded())
        {
            movementStateMachine.ChangeState(player.Idle);
            yield break;
        }
        else
        {
            movementStateMachine.ChangeState(player.Falling);
            yield break;
        }

    }

    public override void OnExit()
    {
        exitingState = false;
        player.Animator.SetBool(heavyDamageAnim, false);
        player.Animator.SetBool(lightDamageAnim, false);
    }

    public override string ToString()
    {
        return "Hurt";
    }
}
