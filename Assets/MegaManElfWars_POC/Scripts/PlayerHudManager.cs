using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudManager : Singleton<PlayerHudManager>
{
    [SerializeField] private float _playerHealth;
    [SerializeField] private int _weaponEnergy;
    [SerializeField] private int _currentLives;

    private Slider healthFill;
    private Slider weaponFill;
    private Text lives;

    private const string PlayerHealthSlider = "PlayerHealthSlider";
    private const string PlayerLivesText = "PlayerLivesText";
    private PlayerController _player;
    private float healthContainer;
    private void OnEnable()
    {
        healthFill = GameObject.FindGameObjectWithTag(PlayerHealthSlider).GetComponent<Slider>();
        lives = GameObject.FindGameObjectWithTag(PlayerLivesText).GetComponent<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _playerHealth = _player.currentHealth;
        _currentLives = _player.lives;
    }

    // Update is called once per frame
    void Update()
    {
        healthFill.value = _playerHealth;
        lives.text = _currentLives.ToString();
    }

    public void SetHealth(float damage)
    {
        _playerHealth = Mathf.Lerp(_playerHealth, _playerHealth - damage, 5f);
    }

    public void ResetHealth() => _playerHealth = _player.MovementData.health;

    public void SetMaxHealth(float maximum) => healthFill.maxValue = maximum;

    public void DecrementLives(int amount)
    {
        _currentLives -= amount;
        _currentLives = Mathf.Max(_currentLives, 0);
    }

    public void AddLives(int amount)
    {
        _currentLives += amount;
        _currentLives = Mathf.Max(_currentLives, 9);
    }

    public void SetPlayer(PlayerController player)
    {
        _player = player;
        _playerHealth = _player.currentHealth;
    }
}
