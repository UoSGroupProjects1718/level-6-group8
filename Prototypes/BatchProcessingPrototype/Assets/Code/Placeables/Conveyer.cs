using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : Placeable
{
    Ingredient newChild;
    Ingredient child;

    void Start()
    { }

    void Update()
    { }

    public override void Tick()
    {
        LevelController lc = GameObject.Find("LevelController").GetComponent<LevelController>();

        int neighbourY = y;
        int neighbourX = x;

        switch (dir)
        {
            case Direction.up:
                neighbourY++;
                break;
            case Direction.right:
                neighbourX++;
                break;
            case Direction.down:
                neighbourY--;
                break;
            case Direction.left:
                neighbourX--;
                break;
        }

        if (lc.currentLevel[neighbourY, neighbourX] == null)
        {
            lc.StopRunning("Conveyer belt passing ingredients up onto nothing.", true);
        }
        else
        {
            lc.currentLevel[neighbourY, neighbourX].GetComponent<Placeable>().GiveIngredient(child);
            child = null;
        }
    }

    public override void GiveIngredient(Ingredient newIngredient)
    {
        if (newChild != null)
        {
            GameObject.Find("LevelController").GetComponent<LevelController>()
                .StopRunning("Two ingredients are being passed onto a conveyer belt in the same tick.", true);
            return;
        }

        newChild = newIngredient;
    }

    public override void Flush()
    {
        child = newChild;
        newChild = null;

        child.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }
}
