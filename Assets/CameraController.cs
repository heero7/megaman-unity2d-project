using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;

    private void OnEnable()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        var loc = FindObjectOfType<Checkpoint>().transform;
        _camera.m_Follow = loc;
        _camera.m_LookAt = loc;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
