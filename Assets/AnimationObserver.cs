using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObserver : MonoBehaviour
{
    private PlayerController _player;
    public static event Action IntroAnimationEnded;

    private void Awake() 
    {
        _player = GetComponentInParent<PlayerController>();    
    }

    public void IntroAnimationOver()
    {
        // Tell the Player Controller that its done.
        IntroAnimationEnded?.Invoke();
    }
}
