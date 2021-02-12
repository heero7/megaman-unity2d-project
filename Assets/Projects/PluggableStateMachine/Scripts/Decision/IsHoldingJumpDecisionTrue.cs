using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/TimeElapsed")]
public class TimeElapsedDecision : Decision
{
    public float time;

    
    public override bool Decide(PluggableStateMachineController stateMachine)
    {
        return EvaluateTime();
    }

    private bool EvaluateTime()
    {
        return false;
    }
}
