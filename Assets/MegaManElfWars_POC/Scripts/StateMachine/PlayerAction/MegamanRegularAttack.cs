using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanRegularAttack : PlayerActionState
{
    public Animator charge1, charge2;
    public MegamanRegularAttack(PlayerController player, StateMachine<PlayerActionState> actionStateMachine, StateMachine<PlayerMovementState> movementStateMachine) : base(player, actionStateMachine, movementStateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.Animator.SetFloat("RegularAttack", 1);
    }

    /**
        SetFloat.. if using Time.deltaTime will use Update() to decrement the value.
        So this doesn't happen asynchronously
    */
    int next;
    bool done = false;
    int count = 0;
    public override void OnExecute()
    {
        base.OnExecute();
        // Currently the button is still being held.
        // Begin Charging the weapon.
        if (player.InputController.AttackPressed && !done)
        {
            // Charging.
            player.Animator.SetFloat(animName, 0, 1f, Time.deltaTime);
            count++;
            if (count > 30) charge1.SetTrigger("ChargeLevel1");
            if (count > 100) charge2.SetTrigger("ChargeLevel2");
            Debug.Log("Charging animation..");
        }

        if (!player.InputController.AttackPressed && !done) // We'll want to check a different context for chargin.
        {
            player.Animator.SetFloat(animName, 1);
            next = 50;
            done = true;
            charge1.ResetTrigger("ChargeLevel1");
            charge2.ResetTrigger("ChargeLevel2");
            count = 0;
            //actionStateMachine.ChangeState(player.NoAction);
            Debug.Log("[Action] Regular Shot!");
            return;
        }

        // Can this be replaced with a CoRoutine??
        // What if instead.. the CoRoutine held the switch?
        if (next == 0 && done)
        {
            actionStateMachine.ChangeState(player.NoAction);
            Debug.Log("Exiting state.");
        }
        else
        {
            next--;
        }
    }

    const string animName = "RegularAttack";
    private IEnumerator WaitForSetFloat(float val, float dur)
    {
        while (player.Animator.GetFloat(animName) > 0)
        {
            player.Animator.SetFloat(animName, val, dur, Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }



    public override void OnExit()
    {
        base.OnExit();
        done = false;
    }

    public override string ToString()
    {
        return "Attack";
    }
}
