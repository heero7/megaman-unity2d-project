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
    public MoveInputEvent _moveInputEvent;
    public JumpInputEvent _jumpInputEvent;

    private void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Move.performed += OnMovePerformed;
        controls.Gameplay.Move.canceled += OnMovePerformed;
        controls.Gameplay.Jump.started += OnJumpPerformed;

        _jumpInputEvent = new JumpInputEvent();
        _moveInputEvent = new MoveInputEvent();

        SpawnManager.PlayerSpawnedEvent += SetupPlayerControls;
    }


    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        _moveInputEvent.Invoke(input.x, input.y);
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        _jumpInputEvent.Invoke();
    }

    private void SetupPlayerControls(RaycastPlayer p)
    {
        _moveInputEvent.AddListener(p.OnMoveInput);
    }

    private void OnDisable() 
    {
        controls.Gameplay.Move.performed -= OnMovePerformed;
        controls.Gameplay.Move.canceled -= OnMovePerformed;
        controls.Gameplay.Jump.started -= OnJumpPerformed;

        _moveInputEvent.RemoveAllListeners();
        controls.Gameplay.Disable();
    }

    private void Awake()
    {
        controls = new RaycastInput();
    }
}
