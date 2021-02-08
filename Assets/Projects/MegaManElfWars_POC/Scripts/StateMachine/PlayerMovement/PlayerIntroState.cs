using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntroState : PlayerMovementState
{
    private bool animationEnded;

    public PlayerIntroState(PlayerController player, StateMachine<PlayerMovementState> movementStateMachine, string animationName) : base(player, movementStateMachine, animationName)
    {
        //AnimationObserver.IntroAnimationEnded += AnimationEnded;
    }

    public override void ApplyGravity()
    {
        player.Rigidbody2D.AddForce(Vector2.down * player.Gravity);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //player.TurnOffSpriteRenderer();
    }

    public override void OnExecute()
    {
        if (animationEnded)
        {
            movementStateMachine.ChangeState(player.Idle);
        }
    }

    private void AnimationEnded() => animationEnded = false;

    public override void OnExit()
    {
        base.OnExit();
        animationEnded = false;
    }

    public override string ToString()
    {
        return "Entering stage.";
    }
}
