using System;
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
        Machine neighbour = LevelController.Instance.GetNeighbour(parent.X, parent.Y, dir);

        // Null check
        if (neighbour == null) { Debug.Log("Null neighbour: returning."); return; }

        // Pass item

        // Instantiate the item
        Item toPass = Instantiate(ItemToOutput.gameObject).GetComponent<Item>();
        
        // Position and rotate the item
        toPass.transform.position = new Vector3(transform.position.x, toPass.ProductionLine_YHeight, transform.position.z);
        toPass.transform.localRotation = Quaternion.Euler(toPass.ProductionLine_Rotation); // Rotate(Item.rotation);// = Quaternion.Euler(Item.rotation);
        toPass.transform.localScale = toPass.ProductionLine_Scale;

        // Add it to our list of tiems
        AddItem(ref toPass);

        // Pass it to our neighbour
        neighbour.Receive(ref toPass);  
    }

    public override void Flush()
    {
        return;
    }

    public override void Execute()
    {
        return;
    }

    public override void Receive(ref Item newItem)
    {
        return;
    }

    public override void Reset()
    {
        return;
    }

    private void OnMouseDown()
    {
        if (LevelController.Instance.BuildStatus != BuildStatus.delete)
        {
            LevelController.Instance.SelectedInputter = this;
            GameObject.Find("Canvas").GetComponent<GameCanvas>().LoadIngredientList();
        }
    }
}
