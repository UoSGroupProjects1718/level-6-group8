using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient : MonoBehaviour
{
    public static List<Ingredient> allIngredients = new List<Ingredient>();

    [SerializeField]
    string IngredientName;

    public string GetName() { return IngredientName; }

    void Start()
    {
        allIngredients.Add(this);
    }
}