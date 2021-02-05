using UnityEngine;
using System.Collections.Generic;
using System;

public class AfterImagePool : Singleton<AfterImagePool>
{
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private int poolGrowthSize = 10;
    private Queue<GameObject> availableObjects = new Queue<GameObject>(); // Stores all objects not made that are currently active.
    public new static AfterImagePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    // Grow the pool if its too small.
    private void GrowPool()
    {
        var startingAlpha = .85f;
        for (int i = 0; i < 10; i++)
        {
            startingAlpha -= .1f;
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.GetComponent<AfterImageController>().SetAlphaOnEnable(startingAlpha);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instanceToAdd)
    {
        instanceToAdd.SetActive(false);
        availableObjects.Enqueue(instanceToAdd);
    }

    public GameObject RetrieveAfterImageFromPool()
    {
        if (availableObjects.Count == 0)
        {
            GrowPool();
        }

        var instance = availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
