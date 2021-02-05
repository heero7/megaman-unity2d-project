using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanRegularAttack : PlayerActionState
{
    public Animator charge1, charge2;
    private const string muzzle1Anim = "xbuster_muzzle1";
    private const string muzzle2Anim = "xbuster_muzzle2";
    private const string muzzle3Anim = "xbuster_muzzle3";

    public MegamanRegularAttack(PlayerController player, StateMachine<PlayerActionState> actionStateMachine, StateMachine<PlayerMovementState> movementStateMachine) : base(player, actionStateMachine, movementStateMachine)
    {
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

    #region Attempt 1
    /*
public override void OnEnter()
{
    base.OnEnter();

    count = 0;
    player.Animator.SetFloat("RegularAttack", 1);
    player.muzzleAnimator.Play(muzzle1Anim);
    player.xBuster.FireBuster(0);
    start = Time.time;
}
*/
    /*public override void OnExecute()
    {
        // Think about refactoring to this!
        // totalCharge += Time.deltaTime;
        base.OnExecute();
        // If we take damage. Immediately change to prevent fire.
        if (movementStateMachine.CurrentState.ToString().Equals("Hurt"))
        {
            actionStateMachine.ChangeState(player.NoAction);
            return;
        }
        // Currently the button is still being held.
        // Begin Charging the weapon.
        // TODO Wait for a few seconds before charging.
        count++;
        if (_attackHeld && !done  && Time.time > start + 2f)
        {
            // Charging.
            player.Animator.SetFloat(animName, 0, 1f, Time.deltaTime);
            //count++;
            if (count > 100) charge1.SetBool("ChargeLevel1", true);
            if (count > 300) charge2.SetBool("ChargeLevel2", true);
        }

        // Wait for a few seconds before charging.
        if (!_attackHeld && !done && Time.time > start + 2f)
        {
            charge1.SetBool("ChargeLevel1", false);
            charge2.SetBool("ChargeLevel2", false);
            player.Animator.SetFloat(animName, 1);
            var level = 0;
            Debug.Log(count);
            if (count < 299 && count > 50) 
            {
                player.muzzleAnimator.Play(muzzle2Anim);
                level = 1;
            }
            else if (count > 300) 
            {
                player.muzzleAnimator.Play(muzzle3Anim);
                level = 2;
            }
            player.xBuster.FireBuster(level);
            Debug.Log(level);
            next = 50;
            done = true;
            count = 0;
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
    
    public override void OnExit()
    {
        base.OnExit();
        done = false;
    }*/
    #endregion

    #region Attempt 2
    private float chargeAmount = 0f;
    private float amountNeededLevel1 = 1.0f;
    private float amountNeededLevel2 = 2.0f;
    private const string chargeLevel1AnimName = "ChargeLevel1";
    private const string chargeLevel2AnimName = "ChargeLevel2";
    public override void OnEnter()
    {
        base.OnEnter();
        player.xBuster.FireBuster(0);
        player.Animator.SetFloat(RegularAttackAnimName, 1);
        player.muzzleAnimator.Play(muzzle1Anim);
    }
    public override void OnExecute()
    {
        base.OnExecute();

        if (_attackHeld)
        {
            player.Animator.SetFloat(RegularAttackAnimName, 0, 1, Time.deltaTime);
            chargeAmount += Time.deltaTime;
            var isHeldForLevel1 = chargeAmount > amountNeededLevel1;
            charge1.SetBool(chargeLevel1AnimName, isHeldForLevel1);

            var isHeldForLevel2 = chargeAmount > amountNeededLevel2;
            charge2.SetBool(chargeLevel2AnimName, isHeldForLevel2);
        }

        /**
            NOTES
            If on a ladder, the Y velocity should temporarily stop.
        */

        if (!_attackHeld)
        {
            player.Animator.SetFloat(RegularAttackAnimName, 1);
            charge1.SetBool(chargeLevel1AnimName, false);
            charge2.SetBool(chargeLevel2AnimName, false);
            //player.StartCoroutine(Wait(2f));
            //Debug.Log("Did this still run?");
            if (chargeAmount < amountNeededLevel1)
            {
                // Do something...?
                // Set any variables that pertain to this here maybe?
            }
            else if (chargeAmount > amountNeededLevel1 && chargeAmount < amountNeededLevel2)
            {
                //Debug.Log("Level2");
                player.xBuster.FireBuster(1);
                player.muzzleAnimator.Play(muzzle2Anim);
            }
            else if (chargeAmount > amountNeededLevel2)
            {
                //Debug.Log("Level3");
                player.xBuster.FireBuster(2);
                player.muzzleAnimator.Play(muzzle3Anim);
            }
            chargeAmount = 0f;
            //player.StartCoroutine(OnStateChangeWait(2f));
            actionStateMachine.ChangeState(player.NoAction);
        }
    }

    public override void OnExit()
    {
        // Here you should try to setFloat on the animation.
        
        base.OnExit();
    }
    #endregion

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
        Debug.Log("Before");
        yield return new WaitForSeconds(time);
        Debug.Log($"{time} second(s) after");
    }

    private IEnumerator OnStateChangeWait(float time)
    {
        yield return new WaitForSeconds(time);
        actionStateMachine.ChangeState(player.NoAction);
    }


    public override string ToString()
    {
        return "Attack";
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Do logic when we get hurt by something.
    }
}
