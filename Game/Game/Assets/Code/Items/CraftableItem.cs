using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableItem : Item
{
    private float concentration;

    [Header("Craftable variables")]
    [SerializeField]
    List<Item> ingredients;

    public float Concentration
    {
        get { return concentration; }
        set { concentration = value; }
    }

    public List<Item> Ingredients
    {
        get { return ingredients; }
    }

    void Start ()
    {
		
	}
	
    /// <summary>
    /// Changes the concentration of a craftableItem by a set amount.
    /// Cannot go below 0 or above 1.
    /// </summary>
    /// <param name="val">How much to change concentration by</param>
    public void AdjustConcentration(float val)
    {
        concentration += val;
        if (concentration > 1 ) { concentration = 1; }
        if (concentration < 0) { concentration = 0; }
    }
}
