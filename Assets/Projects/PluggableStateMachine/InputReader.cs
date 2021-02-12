using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "PluggableStateMachine/InputReader")]
public class InputReader : ScriptableObject, PSInputActions.IGameplayActions
{
    public event UnityAction<Vector2> moveEvent = delegate { };
    public event UnityAction jumpEvent = delegate { };

    private PSInputActions receiver;

    private void OnEnable()
    {
        if (receiver == null)
        {
            receiver = new PSInputActions();
            receiver.Gameplay.SetCallbacks(this);
        }

        // Enable gameplay input
        receiver.Gameplay.Enable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {   
        if (context.phase == InputActionPhase.Performed)
            jumpEvent.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            moveEvent.Invoke(context.ReadValue<Vector2>());
        if (context.phase == InputActionPhase.Canceled)
            moveEvent.Invoke(context.ReadValue<Vector2>());
    }

    void OnDisable()
    {
        receiver.Gameplay.Disable();
    }
}
