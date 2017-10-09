using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoPause {

    // Prefab to spawn
    public GameObject spawningObject;
    public Vector3 spawnOffset;

    // Time to spawn
    public float spawnTime;
    public float currentTime;

    private Vector3 spawnPosition;

    protected override void DoStart()
    {
        currentTime = 0.0f;
        spawnPosition = this.transform.position + spawnOffset;
    }

    protected override void DoUpdate()
    {
        currentTime += Time.deltaTime;

        if(currentTime > spawnTime)
        {
            //spawn object
            ObjectPooling.objectsToPause.Add(Instantiate(spawningObject, spawnPosition, Quaternion.identity));
            currentTime = 0.0f;
        }
    }
}
