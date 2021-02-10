using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static event Action OnReadyActionEnd;
    private PlayableDirector playable;

    void OnEnable()
    {
        playable = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        // Nasty...
        var p = FindObjectOfType<RaycastPlayer>();
        if (p != null) return;
        playable.Play();
    }

    public void ReadyActionEnd()
    {
        OnReadyActionEnd?.Invoke();
    }
}
