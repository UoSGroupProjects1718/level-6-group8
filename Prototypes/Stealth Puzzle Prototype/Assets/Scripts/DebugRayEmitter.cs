using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRayEmitter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position, transform.forward * 100, Color.green);
	}
}
