using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class MoveInputEvent : UnityEvent<float, float> { }

[Serializable]
public class JumpInputEvent : UnityEvent { }

public class RaycastInputController : MonoBehaviour
{
    private RaycastInput controls;
    public MoveInputEvent MoveInputEvent;
    public JumpInputEvent JumpInputEvent;

    private void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Move.performed += OnMovePerformed;
        controls.Gameplay.Move.canceled += OnMovePerformed;
        controls.Gameplay.Jump.started += OnJumpPerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        MoveInputEvent.Invoke(input.x, input.y);
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        JumpInputEvent.Invoke();
    }

    private void OnDisable() 
    {
        controls.Gameplay.Move.performed -= OnMovePerformed;
        controls.Gameplay.Move.canceled -= OnMovePerformed;
        controls.Gameplay.Jump.started -= OnJumpPerformed;
        controls.Gameplay.Disable();
    }

    private void Awake()
    {
        controls = new RaycastInput();
    }
}
