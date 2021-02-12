using System;
using UnityEngine;

/// <summary>
/// IsGroundedDecision class. Checks if the controller
/// is touching the ground.
/// </summary>
[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/IsGrounded")]
public class IsGroundedDecision : Decision
{
    public override bool Decide(PluggableStateMachineController stateMachine) => IsGrounded(stateMachine);

    private bool IsGrounded(PluggableStateMachineController stateMachine)
    {
        return stateMachine.CharacterController.CollisionInfo.Below;
    }
}
