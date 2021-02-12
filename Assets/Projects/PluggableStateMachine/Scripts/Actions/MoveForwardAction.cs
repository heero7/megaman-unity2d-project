using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// MoveForwardAction class. Applies movement
/// in the direction requested.
/// </summary>
[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/MoveFoward")]
public class MoveForwardAction : StateAction
{
    public override void Perform(PluggableStateMachineController statemachine)
    {
        MoveFoward(statemachine);
    }

    // Create a virtual input manager like Roundbear?
    // Link: https://www.youtube.com/watch?v=HVwss7AFfaA&list=PLWYGofN_jX5BupV2xLjU1HUvujl_yDIN6&index=15
    // This is interesting, because this solves the problem of creating
    // a whole separate slew of Actions/States/Transitions/Conditions.
    // There only has to be one predefined number of actions, anyone can
    // do and you just have to give them the option.
    // Pig Chef solution
    // Update the parameters of the movement script / input controlling script
    // Example, this class can have access to the CharacterController.InputScript
    // Then it can set values from that script CharacterController.InputScript.MovementVector.y = 3f;
    // And this doesn't have to worry about calling in a Function like Move(parm1, parma2, param3) 
    private void MoveFoward(PluggableStateMachineController stateMachine)
    {
        stateMachine.Player.movementVector.x = stateMachine.Player.CharacterData.moveSpeed * stateMachine.Player.MovementInput.x;
    }
}
