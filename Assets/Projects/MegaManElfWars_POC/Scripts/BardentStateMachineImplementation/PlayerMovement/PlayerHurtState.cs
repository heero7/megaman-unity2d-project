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

    public PlayerHurtState(PlayerController player, BardentStateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
    }

    public Vector2 startingPosition;
    public Vector2 endPosition;
    public Vector2 topPoint;
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

        // Bezier curve loop
        startingPosition = player.transform.position;
        endPosition = new Vector2(player.transform.position.x, player.transform.position.y) + Vector2.right * -player.FacingDirection * 2;
        topPoint = Vector2.up * .1f;
    }

    bool once;
    float count = 0.0f;

    public override void OnExecute()
    {
        timeInState += Time.deltaTime;

        if (once && !aLotOfDamageTaken) 
        {
            player.Rigidbody2D.AddForce(new Vector2(-player.FacingDirection * 3, 0), ForceMode2D.Impulse);
            once = false;
        }
        else if (aLotOfDamageTaken)
        {
            if (count < 1.0f) 
                count += 1.0f *Time.deltaTime;
            var m1 = Vector2.Lerp(startingPosition, topPoint, count);
            var m2 = Vector2.Lerp(topPoint, endPosition, count);
            player.transform.position = Vector2.Lerp(m1, m2, count);
        }

        if ((timeInState >= player.MovementData.hurtTime && !aLotOfDamageTaken) 
            || (timeInState >= player.MovementData.hurtTime * 3 && aLotOfDamageTaken))
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
        aLotOfDamageTaken = false;
        count = 0;
        player.Animator.SetBool(heavyDamageAnim, false);
        player.Animator.SetBool(lightDamageAnim, false);
    }

    public override string ToString()
    {
        return "Hurt";
    }
}
