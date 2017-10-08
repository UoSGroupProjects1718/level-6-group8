using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {

    public static List<GameObject> objectsToPause;

	// Use this for initialization
	void Start () {
        objectsToPause = new List<GameObject>();
	}

    public static void PauseObjects(bool state)
    {
        foreach (GameObject obj in objectsToPause)
        {
            obj.GetComponent<Rigidbody>().isKinematic = state;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
