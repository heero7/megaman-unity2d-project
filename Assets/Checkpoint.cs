using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool _cannotReturn = false; // If this is set, the player can't go backwards.

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Draw an icon... like a flag at this location.
        LevelManager.Instance.SetNextCheckPoint(transform);
        // Set active false
    }
}
