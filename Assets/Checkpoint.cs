using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool _cannotReturn = false; // If this is set, the player can't go backwards.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return; // Wow.
        // Draw an icon... like a flag at this location.
        Debug.Log($"Updating the checkpoint: {transform.position}");
        LevelManager.Instance.SetNextCheckPoint(transform);
        // Set active false
    }
}
