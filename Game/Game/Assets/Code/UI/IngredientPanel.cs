using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientPanel : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField]
    Image image;

    [Header("Name text")]
    [SerializeField]
    Text displayName;

    private Ingredient ingredient;

    public void SetInfo(Ingredient ingredientArg)
    {
        // Remember this panels ingredient
        ingredient = ingredientArg;

        image.sprite = ingredient.ItemSprite;
        displayName.text = ingredient.DisplayName;
    }

    public void SetInputIngredient()
    {
        GameObject.Find("LevelController").GetComponent<LevelController>().UpdateSelectedInputtersIngredient(ingredient);
    }

    public void CloseIngredientList()
    {
        GameObject.Find("Canvas").GetComponent<GameCanvas>().CloseIngredientList();
    }
}
