using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/ApplyMovement")]
public class ApplyMovementVectorAction : StateAction
{
    public override void Perform(PluggableStateMachineController controller)
    {
        ApplyMovement(controller);
    }

    private void ApplyMovement(PluggableStateMachineController stateMachine)
    {
        stateMachine.CharacterController.Move(stateMachine.Player.movementVector * Time.deltaTime, false);
    }
}
