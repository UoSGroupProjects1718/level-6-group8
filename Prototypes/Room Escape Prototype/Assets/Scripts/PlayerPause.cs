using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPause : MonoBehaviour {

    public SpawnObject sObject;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            MonoPause.pause = !MonoPause.pause;

            ObjectPooling.PauseObjects(MonoPause.pause);
            PlayerController.useGravity = !MonoPause.pause;
        }
	}
}
