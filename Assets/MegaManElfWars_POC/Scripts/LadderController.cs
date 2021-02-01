using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var o = other.gameObject;
        var input = Input.GetAxisRaw("Vertical");
        if (other.CompareTag("upperLadder") && input == 1)
        {
            // Do stuff when reaching the top.
            Debug.Log("Head touched the top");
            // This is where we need to be at the moment.
            var player = other.gameObject.GetComponentInParent<PlayerController>();
            //player.MoveInGrid(other.transform.position);
            //player.StopClimbing();
        }
        if (other.CompareTag("lowerLadder") && input == -1)
        {
            // Do stuff when reaching the bottom.
            Debug.Log("Feet touched the bottom");
            var player = other.gameObject.GetComponentInParent<PlayerController>();
            //player.StopClimbing();
        }
    }
}
