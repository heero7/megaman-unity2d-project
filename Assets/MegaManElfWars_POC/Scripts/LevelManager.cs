using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private PlayerController _currentPlayer; // X, Axl, or Zero.
    private Transform currentCheckPoint;
    private const string FirstCheckPointTag = "First-CheckPoint";

    private void Awake()
    {

    }

    private void OnEnable()
    {
        // Get the first check point, when loading.
        var checkPoint = GameObject.FindGameObjectWithTag(FirstCheckPointTag).GetComponent<Checkpoint>();
        currentCheckPoint = checkPoint.transform;
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
        Debug.Log("Player is attempting respawn...");
        // ... You could resource.Load this.
        Instantiate(_currentPlayer, currentCheckPoint.position, currentCheckPoint.rotation);
    }

    public void SetNextCheckPoint(Transform pos) => currentCheckPoint = pos;
}
