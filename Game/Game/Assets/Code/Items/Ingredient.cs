using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item
{
    [Header("Cooked variant")]
    [SerializeField]
    private Ingredient cookedVariant;

    public Ingredient CookedVariant { get { return cookedVariant; } }
}
