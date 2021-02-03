using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float fireRate;
    private int direction;
    private bool wallSliding;
    private PlayerController p;
    [SerializeField] Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        p = FindObjectOfType<PlayerController>();
        direction = p.FacingDirection;
                wallSliding = p.MovementStateMachine.CurrentState.ToString().Equals("WallSlide");
        direction = p.FacingDirection;
        if (wallSliding) direction *= -1;
        
        rb.velocity = transform.right * direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {   
        if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
