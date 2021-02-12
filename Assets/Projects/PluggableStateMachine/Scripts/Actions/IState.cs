public interface IStateAction
{
    void Entrance(PluggableStateMachineController statemachine);
    void Perform(PluggableStateMachineController statemachine);
    void Exit(PluggableStateMachineController statemachine);
}