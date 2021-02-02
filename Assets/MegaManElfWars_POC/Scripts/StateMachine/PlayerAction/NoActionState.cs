using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoActionState : PlayerActionState
{
    public NoActionState(PlayerController player, StateMachine<PlayerActionState> actionStateMachine, StateMachine<PlayerMovementState> movementStateMachine) : base(player, actionStateMachine, movementStateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.Animator.SetFloat("RegularAttack", 0);
    }

    public override void OnExecute()
    {
        base.OnExecute();

        if (player.InputController.AttackPressed)
        {
            actionStateMachine.ChangeState(player.Attack);
            return;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override string ToString()
    {
        return "Not Acting";
    }
}
