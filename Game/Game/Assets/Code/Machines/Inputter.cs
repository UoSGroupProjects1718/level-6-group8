using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputter : Machine
{
    [SerializeField]
    [Header("Output item")]
    public Item ItemToOutput;

	void Start ()
    {
        lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        ResetTickCounter();
	}

    public void SetOutputItem(Item item)
    {
        ItemToOutput = item;
    }
	
    public override void Tick()
    {
        // Tick +
        tickCounter++;
        if (tickCounter < ticksToExecute) { return; }

        ResetTickCounter();

        // Get the machine im facing
        Machine neighbour = lc.GetNeighbour(parent.X, parent.Y, dir);

        // Null check
        if (neighbour == null) { Debug.Log("Null neighbour: returning."); return; }

        // Pass item
        Item toPass = Instantiate(ItemToOutput.gameObject).GetComponent<Item>();
        AddItem(ref toPass);
        neighbour.Receive(toPass);  
    }

    public override void Flush()
    {
        return;
    }

    public override void Execute()
    {
        return;
    }

    public override void Receive(Item newItem)
    {
        return;
    }

    public override void Reset()
    {
        return;
    }
}
