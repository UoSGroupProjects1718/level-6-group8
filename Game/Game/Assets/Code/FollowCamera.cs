using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Vector3 cameraPos;
    private float strength = 10.5f;

	// Update is called once per frame
	void Update () {
       // cameraPos = Camera.main.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - transform.position), Mathf.Min(strength * Time.deltaTime, 1));
    }
}
