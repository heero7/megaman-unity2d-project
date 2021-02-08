using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FinishedClimbingEventPosition
{
    Top,
    Bottom
}
public class LadderController : MonoBehaviour
{
    public static event Action<FinishedClimbingEventPosition> FinishedClimbingEvent;
    public event Action DamagedWhileClimbing;

    void OnTriggerExit2D(Collider2D other)
    {
        // Test.

        var player = other.gameObject.GetComponentInParent<PlayerController>();
        if (player == null) return;
        var input = player.InputController.NormalizedInputY;
        if (other.CompareTag("upperLadder") && input == 1) // TODO: Look for a way NOT to have to read the player object. Can't we check some other way?
        {
            // Do stuff when reaching the top.
            FinishedClimbingEvent?.Invoke(FinishedClimbingEventPosition.Top);
            // This is where we need to be at the moment.
            //player.MoveInGrid(other.transform.position);
            //player.StopClimbing();
        }
        if (other.CompareTag("lowerLadder") && input == -1)
        {
            // Do stuff when reaching the bottom.
            FinishedClimbingEvent?.Invoke(FinishedClimbingEventPosition.Bottom);
            //player.StopClimbing();
        }
    }
}
