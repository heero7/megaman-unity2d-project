using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour, IDamageDealer, IDamageReceiver
{
    [SerializeField] private float damageOnContact = 1f;
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private TextMesh healthDisplay;
    [SerializeField] private GameObject deathFX;
    public float CurrentHealth { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxHealth;
        healthDisplay.text = CurrentHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GiveDamage()
    {
        return damageOnContact;
    }

    public void ReceiveDamage(float damage)
    {
        var curHealth = CurrentHealth;
        curHealth -= damage;
        CurrentHealth =  Mathf.Clamp(curHealth, 0, maxHealth);
        healthDisplay.text = CurrentHealth.ToString();

        // Do something... like maybe check for health.
        CheckHealthStatus();
    }

    private void CheckHealthStatus()
    {
        if (CurrentHealth <= 0)
        {
            // Play the death effect.
            // SImilar to the hit effect on a projectile.
            var o = Instantiate(deathFX, transform.position, transform.rotation);
            GameManager.Instance.CleanUpFX(o);
            Destroy(gameObject);
        }
    }
}
