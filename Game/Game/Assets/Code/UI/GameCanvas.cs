using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    // Dont touch these, they works perfectly
    [Header("Ingredient Selection Parent")]
    [SerializeField]
    GameObject ingredientListParent;

    [Header("Ingredient list scrollable")]
    [SerializeField]
    GameObject ingredientListPanel;

    [Header("Debug text - build mode")]
    public Text debugBuildModeText;

    public void LoadIngredientList()
    {
        ingredientListParent.SetActive(true);
        ingredientListPanel.GetComponent<ScrollableList>().Fill();
    }

    public void CloseIngredientList()
    {
        ingredientListParent.SetActive(false);
    }

    public void Debug_SetBuildModeText(BuildMode bm)
    {
        debugBuildModeText.text = string.Format("Mode: {0}", bm.ToString());
    }
}
