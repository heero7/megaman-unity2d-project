using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;
    public CharacterData CharacterData { get => characterData; }
    [SerializeField] private InputReader _intputReader = default;
    [HideInInspector] public Vector2 MovementInput { get; private set; }
    [NonSerialized] public Vector2 movementVector; // Use this in states.
    [NonSerialized] public bool jumpInput;

    private void OnEnable()
    {
        _intputReader.jumpEvent += OnJump;
        _intputReader.moveEvent += OnMove;
    }

    public void ResetJumpInputCache()
    {
        jumpInput = false;
    }

    private void OnJump()
    {
        jumpInput = true;
    }


    private void OnMove(Vector2 delta)
    {
        MovementInput = delta;
    }

    private void OnDisable()
    {
        _intputReader.jumpEvent -= OnJump;
        _intputReader.moveEvent -= OnMove;
    }
}