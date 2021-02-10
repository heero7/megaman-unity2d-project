using System;
using UnityEngine;
using Cinemachine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private RaycastPlayer player;
    private CinemachineVirtualCamera _camera;
    public static event Action<RaycastPlayer> PlayerSpawnedEvent;
    private Checkpoint spawnPoint;

    void OnEnable()
    {
        TimelineManager.OnReadyActionEnd += FirstSpawnPlayer;
    }

    private void OnDisable()
    {
        TimelineManager.OnReadyActionEnd -= FirstSpawnPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GetComponentInChildren<Checkpoint>();
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    public void FirstSpawnPlayer()
    {
        // Does there already exist another player?
        var existing = FindObjectOfType<RaycastPlayer>();
        if (existing != null)
        {
            Debug.LogWarning("There already exists a player in the game. A player won't be instantiated");
            return;
        }
        var alive = Instantiate(player, spawnPoint.transform.position, Quaternion.identity);
        _camera.m_Follow = alive.transform;
        _camera.m_LookAt = alive.transform;
        PlayerSpawnedEvent?.Invoke(alive);
    }
}
