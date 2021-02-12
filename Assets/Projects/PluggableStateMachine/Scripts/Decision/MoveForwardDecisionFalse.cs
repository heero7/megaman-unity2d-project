using UnityEngine;

/// <summary>
/// MoveForwardDecision class. Condition if 
/// the controller should move forward [horizontally (-x or +x)].
/// </summary>
[CreateAssetMenu(menuName = "PluggableStateMachine/Decisions/MoveForward_False")]
public class MoveForwardDecisionFalse : Decision
{
    public override bool Decide(PluggableStateMachineController stateMachine)
    {
        var commandReceived = HasReceivedHorizontalCommand(stateMachine);
        return commandReceived;
    }

    private bool HasReceivedHorizontalCommand(PluggableStateMachineController stateMachine)
    {
        return stateMachine.Player.MovementInput.x == 0;
    }
}