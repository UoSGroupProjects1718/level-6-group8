using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputer : Placeable
{
    public GameObject ingredient;

    [SerializeField]
    int ticksPerItem;
    private int ticks;

    void Start()
    {
        ticks = 0;
        allPlaceables.Add(this);
    }

    // Passes an ingredient onto the next neighbour
    public override void Tick()
    {
        ticks++;
        if (ticks < ticksPerItem) { return; }

        // Find our neighbours [y,x] index based on our facing direction
        int neighbourX, neighbourY;
        CalculateNeighbour(dir, out neighbourX, out neighbourY);

        LevelController lc = GameObject.Find("LevelController").GetComponent<LevelController>();

        if (lc.currentLevel[neighbourY, neighbourX].GetComponent<Tile>().GetChild() == null)
        {
            lc.StopRunning("Inputter passing ingredients up onto nothing.", true);
        }
        else
        {
            Ingredient toGive = Instantiate(ingredient, transform.position, Quaternion.identity).GetComponent<Ingredient>();
            lc.currentLevel[neighbourY, neighbourX].GetComponent<Tile>().GetChild().GiveIngredient(toGive);
        }

        ticks = 0;
    }

    public override void Flush()
    {
        // Inputter has no flush.
    }

    public override void GiveIngredient(Ingredient newIngredient)
    {
        throw new NotImplementedException();
    }
}
