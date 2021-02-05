using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageController : MonoBehaviour
{
    [SerializeField] private float activeTime = 0.1f; // How long this is active.
    private float timeActivated; // How long this object has been active.
    private float alpha; // The current alpha.
    private float alphaOnEnable = 0.8f; // Alpha when the object is enabled.
    [SerializeField] private float alphaDecay = 0.85f; // The smaller this number, the faster it fades.
    private Transform _target; // The current location of the desired target.
    private SpriteRenderer _spriteRenderer; // SpriteRenderer to show the after image.
    private SpriteRenderer _targetSpriteRenderer;   // SpriteRenderer of the target.
    [SerializeField] private Color color;
    public void SetAlphaOnEnable(float a) => alphaOnEnable = a;

    private void OnEnable() 
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _target = GameObject.FindGameObjectWithTag("Player").transform; // Maybe find a way to make this generic for anything. Maybe get component in parent and look for Controller i.e. RaycastController.
        _targetSpriteRenderer = GameObject.FindGameObjectWithTag("Character").GetComponent<SpriteRenderer>();

        alpha = alphaOnEnable;
        _spriteRenderer.sprite = _targetSpriteRenderer.sprite; // Get the correct sprite on enable.
        
        // Set the transforms
        // If the sprite renderer is on the child object, you can't use the parent.
        // Use the GameObject that has the sprite renderer to get the right position.
        transform.position = _targetSpriteRenderer.transform.position;
        transform.rotation = _target.rotation;

        timeActivated = Time.time;
    }

    private void Update() 
    {
        alpha -= alphaDecay * Time.deltaTime;
        //color = new Color(1f, 1f, 1f, alpha);
        color.a = alpha;

        _spriteRenderer.color = color;

        // Check if the after image has been on for too long.
        if (Time.time >= (timeActivated + activeTime))
        {
            // Add this back to the pool.
            AfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
