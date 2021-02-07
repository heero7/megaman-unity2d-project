using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum Players
{
    X,
    Axl,
    Zero
}
public class LevelManager : Singleton<LevelManager>
{
    private PlayerController _currentPlayer; // X, Axl, or Zero.
    private CinemachineVirtualCamera _camera;
    private Transform currentCheckPoint;
    private const string FirstCheckPointTag = "First-CheckPoint";
    private const string MainVirtualCameraTag = "MainVirtualCamera";

    private void Awake()
    {
        //_currentPlayer = Resources.Load("Players/XBase", typeof(PlayerController)) as PlayerController;
        _currentPlayer = GameObject.FindObjectOfType<PlayerController>();
        _camera = GameObject.FindGameObjectWithTag(MainVirtualCameraTag).GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        // Get the first check point, when loading.
        var checkPoint = GameObject.FindGameObjectWithTag(FirstCheckPointTag).GetComponent<Checkpoint>();
        currentCheckPoint = checkPoint.transform;
        Debug.Log($"Curent Checkpoint: {currentCheckPoint.position}");
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RespawnPlayer()
    {
        var player = Instantiate(_currentPlayer, currentCheckPoint.position, currentCheckPoint.rotation);
        player.currentHealth = player.MovementData.health; // Set health to max.
        _camera.m_Follow = player.transform;
        _camera.m_LookAt = player.transform;
    }

    public void RespawnNoDestroy()
    {
        _currentPlayer.transform.position = currentCheckPoint.position;
        _currentPlayer.currentHealth = _currentPlayer.MovementData.health;
        _currentPlayer.TurnOnSpriteRenderer();
        PlayerHudManager.Instance.ResetHealth();
    }

    public IEnumerator Respawn()
    {
        yield return null;
    }

    public void SetNextCheckPoint(Transform pos) => currentCheckPoint = pos;
}
