using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Output : Placeable
{
    List<Ingredient> outputIngredients;

    public override void Tick()
    {
        // Check for complete
    }

    public override void Flush()
    {
        // Check for complete
    }

    public override void GiveIngredient(Ingredient newIngredient)
    {
        outputIngredients.Add(newIngredient);
    }

    void Start ()
    {
        outputIngredients = new List<Ingredient>();
	}
}
