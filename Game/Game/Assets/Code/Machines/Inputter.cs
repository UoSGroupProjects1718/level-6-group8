﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputter : Machine
{
    [Header("Output item")]
    [SerializeField]
    public Ingredient ItemToOutput;

	void Start ()
    {
        lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        ResetTickCounter();
	}

    public void SetOutputItem(Ingredient item)
    {
        ItemToOutput = item;
        Debug.Log(string.Format("Inputter \"{0}\" item set as: {1}", gameObject.name, ItemToOutput.DisplayName));

        while (ItemToOutput.DisplayName != item.DisplayName)
        {
            ItemToOutput = item;
        }
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
        toPass.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
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

    private void OnMouseDown()
    {
        if (lc.BuildStatus != BuildStatus.delete)
        {
            GameObject.Find("LevelController").GetComponent<LevelController>().SelectedInputter = this;
            GameObject.Find("Canvas").GetComponent<GameCanvas>().LoadIngredientList();
        }
    }
}
