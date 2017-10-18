using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Output : Machine {
    Item item;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Tick()
    {
        
    }

    public override void Flush()
    {

    }

    public override void Execute()
    {
        Destroy(item.gameObject);
        item = null;
    }

    public override void Receive(Item newItem)
    {
        item = newItem;
    }
}
