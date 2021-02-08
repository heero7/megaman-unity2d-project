using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour, IDamageDealer
{
    public float speed;
    public float damage;
    private int direction;
    private bool wallSliding;
    private PlayerController p;
    private Rigidbody2D rb;
    [SerializeField] GameObject fx;
    private SpriteRenderer spriteRenderer;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
        if (other.CompareTag("Ground") || other.CompareTag("Enemy"))
        {
            
            var opposition = other.GetComponent<IDamageReceiver>();
            if (opposition != null)
            {
                opposition.ReceiveDamage(damage);
            }
            
            if (direction == -1)
            {
                // on hit effect
                var o = Instantiate(fx, transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
                GameManager.Instance.CleanUpFX(o);
            }
            else 
            {
                // on hit effects
                var o = Instantiate(fx, transform.position, transform.rotation);
                GameManager.Instance.CleanUpFX(o);
            }
            
            Destroy(gameObject);
        }
    }

    public float GiveDamage()
    {
        return damage;
    }
}
