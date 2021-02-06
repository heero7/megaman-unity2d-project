using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyController : MonoBehaviour, IDamageDealer, IDamageReceiver
{
    [SerializeField] private float damageOnContact = 1f;
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float flashTime = 0.5f;
    [SerializeField] private TextMesh healthDisplay;
    [SerializeField] private GameObject deathFX;
    private Material defaultMaterial, flashMaterial;
    private SpriteRenderer _spriteRenderer;

    public float CurrentHealth { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        flashMaterial = Resources.Load("HitFlash", typeof(Material)) as Material;
        defaultMaterial = _spriteRenderer.material;
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
        StartCoroutine(Flash());
        healthDisplay.text = CurrentHealth.ToString();

        // Do something... like maybe check for health.
        CheckHealthStatus();
    }

    private IEnumerator Flash()
    {
        _spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashTime);
        _spriteRenderer.material = defaultMaterial;
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
