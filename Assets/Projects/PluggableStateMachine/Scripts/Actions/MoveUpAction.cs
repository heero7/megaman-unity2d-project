using UnityEngine;

[CreateAssetMenu(menuName = "PluggableStateMachine/Actions/MoveUp")]
public class MoveUpAction : StateAction
{
    public override void Perform(PluggableStateMachineController statemachine)
    {
        MoveUp(statemachine);
    }

    private void MoveUp(PluggableStateMachineController statemachine)
    {
        statemachine.Player.movementVector.y = statemachine.Player.CharacterData.jumpHeight;
    }
}
