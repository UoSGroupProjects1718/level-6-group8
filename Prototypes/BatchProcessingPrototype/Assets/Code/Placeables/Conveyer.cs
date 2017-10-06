using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : Placeable
{
    Ingredient newChild;

    [SerializeField]
    Ingredient child;

    void Start()
    {
        child = null;
        allPlaceables.Add(this);
    }

    // Passes its child onto the next neighbour
    public override void Tick()
    {
        // Return if we don't have a child go pass
        if (child == null) { return; }

        // Find our neighbours [y,x] index based on our facing direction
        int neighbourX, neighbourY;
        CalculateNeighbour(dir, out neighbourX, out neighbourY);

        Debug.Log("Attempting to give ingredient to: y/x: " + neighbourX + "/" + neighbourY);

        LevelController lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        if (lc.currentLevel[neighbourY, neighbourX].GetComponent<Tile>().GetChild() == null)
        {
            lc.StopRunning("Conveyer belt passing ingredients up onto nothing.", true);
        }
        else
        {
            lc.currentLevel[neighbourY, neighbourX].GetComponent<Tile>().GetChild().GiveIngredient(child);
            child = null;
        }
    }

    // Sets given ingredient as its newchild
    public override void GiveIngredient(Ingredient newIngredient)
    {

        Debug.Log("Receiving ingredient.");

        if (newChild != null)
        {
            GameObject.Find("LevelController").GetComponent<LevelController>()
                .StopRunning("Two ingredients are being passed onto a conveyer belt in the same tick.", true);
            return;
        }

        newChild = newIngredient;
    }

    // Sets its newchild as its current ingredient
    public override void Flush()
    {
        if (newChild == null) { return; }

        child = newChild;
        newChild = null;

        child.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }
}
