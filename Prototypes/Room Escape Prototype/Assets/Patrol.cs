using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

    public Transform[] waypoints;
    public float speed = 0.5f;

    private int index;
    private Transform currentWaypoint;

	// Use this for initialization
	void Start () {
        index = 0;
        currentWaypoint = waypoints[index];
    }
	
	// Update is called once per frame
	void Update () {
		if(currentWaypoint != null)
        {
            move();
        } else { 
            nextWaypoint();
        }
	}

    void move()
    {
        Vector3 direction = currentWaypoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (direction.magnitude < 2.0f)
            currentWaypoint = null;
    }

    void nextWaypoint()
    {
        index++;
        if(index == waypoints.Length)
        {
            index = 0;
        }
        currentWaypoint = waypoints[index];
    }
}
