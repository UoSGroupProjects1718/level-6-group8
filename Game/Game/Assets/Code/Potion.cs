using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public Potion(List<Ingredient> ingredients)
    {
        this.ingredients = ingredients;
    }

    [SerializeField]
    private List<Ingredient> ingredients;
    public List<Ingredient> Ingredients
    {
        get { return ingredients; }
        set { ingredients = value;  }
    }

    [SerializeField]
    private float concentration;
    public float Concentration
    {
        get
        {
            return concentration;
        }
        set
        {
            concentration = value;
        }
    }
}
