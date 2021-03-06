﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DebugUtility : MonoBehaviour
{
    [SerializeField] Text movementState, actionState, facingDirection, currentHealth;
    [SerializeField] private PlayerController player;
    private Gamepad gamepad;
    private Keyboard keyboard;

    private void Start()
    {
        keyboard = Keyboard.current;
        gamepad = Gamepad.current;
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

        if (player != null)
        {
            movementState.text = player.MovementStateMachine.CurrentState.ToString();
            actionState.text = player.ActionStateMachine.CurrentState.ToString();
            facingDirection.text = player.FacingDirection.ToString();
            currentHealth.text = $"{player.currentHealth}/{player.MovementData.health}";
            if (keyboard == null && gamepad == null) Debug.LogWarning("No recognized input is connected.");
            if ((keyboard != null && keyboard.rKey.isPressed) || (gamepad != null && gamepad.selectButton.isPressed))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (keyboard != null && keyboard.enterKey.wasPressedThisFrame || gamepad != null && gamepad.startButton.wasPressedThisFrame)
            {
                if (Time.timeScale == 0) Time.timeScale = 1;
                else Time.timeScale = 0;
            }
        }
    }
}
