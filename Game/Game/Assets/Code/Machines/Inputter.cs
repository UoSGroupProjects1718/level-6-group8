using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputter : Machine
{
    [Header("Output item")]
    [SerializeField]
    private Item ItemToOutput;

    [Header("Inputter canvas")]
    [SerializeField]
    private UI_InputterPanel panel;

    public Item SelectedItem { get { return ItemToOutput; } }

	void Start ()
    {
        ResetTickCounter();
	}

    public void SetOutputItem(Item item)
    {
        if (item == null)
        {
            ItemToOutput = null;
            return;
        }

        ItemToOutput = item;
        Debug.Log(string.Format("Inputter \"{0}\" item set as: {1}", gameObject.name, ItemToOutput.DisplayName));

        // Update canvas image
        panel.SetImage(item);

        while (ItemToOutput.DisplayName != item.DisplayName)
        {
            ItemToOutput = item;
        }
    }

    /// <summary>
    /// Called upon the production line beginning
    /// </summary>
    public override void Begin() { return; }

    public override void Tick()
    {
        // Tick +
        tickCounter++;
        if (tickCounter < ticksToExecute) { return; }

        ResetTickCounter();

        // Item null check
        if (ItemToOutput == null) { Debug.Log("Null output item: returning."); return; }

        // Get the machine im facing
        Machine neighbour = LevelController.Instance.GetNeighbour(parent.X, parent.Y, dir);

        // Neighbour null check
        if (neighbour == null) { Debug.Log("Null neighbour: returning."); return; }

        // Check neighbour can receive
        if (neighbour.CanReceiveFrom(this))
        {   
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
    }

    public override void Flush()
    {
        return;
    }

    public override void Execute()
    {
        return;
    }

    public override bool CanReceiveFrom(Machine from)
    {
        return false;
    }

    public override void Receive(ref Item newItem)
    {
        return;
    }

    public override void Reset()
    {
        return;
    }

    protected override void OnMachinePress()
    {
        // Inputters can only be deleted in debugDelete mode
        // debugDelete is not available to the player - only used in debugging
        if (LevelController.Instance.BuildStatus == BuildMode.debugdelete)
        {
            DeleteSelf();
        }
        else
        // Normal Behavior
        {
            // Close mayor dialogue
            GameCanvas.Instance.CloseMessage();

            // Open the ingredient list
            LevelController.Instance.SelectedInputter = this;
            GameCanvas.Instance.ToggleIngredientList();
        }
    }
}
