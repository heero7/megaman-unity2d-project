using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    private int direction;
    private bool wallSliding;
    private PlayerController p;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject fx;
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        p = FindObjectOfType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = p.FacingDirection;
                wallSliding = p.MovementStateMachine.CurrentState.ToString().Equals("WallSlide");
        direction = p.FacingDirection;

        if (direction == 1)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction == -1)
        {
            spriteRenderer.flipX = true;
        }

        if (wallSliding)
        {
            direction *= -1;
        } 
        
        rb.velocity = transform.right * direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {   
        if (other.CompareTag("Ground"))
        {
            Instantiate(fx, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
