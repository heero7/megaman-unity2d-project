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
        
        count = 0;
        player.Animator.SetFloat("RegularAttack", 1);
        //player.xBuster.FireBuster(0);
        start = Time.time;
    }

    /**
        SetFloat.. if using Time.deltaTime will use Update() to decrement the value.
        So this doesn't happen asynchronously
    */
    int next;
    bool done = false;
    int count = 0;
    float start;
    float shootDelay = 0.5f;
    public override void OnExecute()
    {
        base.OnExecute();
        // If we take damage. Immediately change to prevent fire.
        if (movementStateMachine.CurrentState.ToString().Equals("Hurt"))
        {
            actionStateMachine.ChangeState(player.NoAction);
            return;
        }
        // Currently the button is still being held.
        // Begin Charging the weapon.
        count++;
        if (player.InputController.AttackPressed && !done)
        {
            // Charging.
            player.Animator.SetFloat(animName, 0, 1f, Time.deltaTime);
            //count++;
            if (count > 100) charge1.SetTrigger("ChargeLevel1");
            if (count > 300) charge2.SetTrigger("ChargeLevel2");
        }

        // Wait for a few seconds before charging.
        if (!player.InputController.AttackPressed && !done) // We'll want to check a different context for chargin.
        {
            charge1.ResetTrigger("ChargeLevel1");
            charge2.ResetTrigger("ChargeLevel2");
            player.Animator.SetFloat(animName, 1);
            var level = 0;
            Debug.Log(count);
            if (count < 299 && count > 50) level = 1;
            else if (count > 300) level = 2;
            player.xBuster.FireBuster(level);
            Debug.Log(level);
            next = 50;
            done = true;
            count = 0;
            //actionStateMachine.ChangeState(player.NoAction);
            return;
        }

        // Can this be replaced with a CoRoutine??
        // What if instead.. the CoRoutine held the switch?
        if (next == 0 && done)
        {
            actionStateMachine.ChangeState(player.NoAction);
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

    private void OnTriggerEnter2D(Collider2D other) {
        // Do logic when we get hurt by something.
    }
}
