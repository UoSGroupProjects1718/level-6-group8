using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    // Dont touch these, they works perfectly
    [Header("Ingredient Selection Parent")]
    [SerializeField]
    GameObject ingredientListParent;
    [Header("Ingredient list scrollable")]
    [SerializeField]
    GameObject ingredientListPanel;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void LoadIngredientList()
    {
        ingredientListParent.SetActive(true);
        ingredientListPanel.GetComponent<ScrollableList>().Fill();
    }

    public void CloseIngredientList()
    {
        ingredientListParent.SetActive(false);
    }
}
