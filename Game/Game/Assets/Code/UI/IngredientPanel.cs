using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientPanel : DimmablePanel
{
    [Header("Sprite")]
    [SerializeField]
    Image image;

    [Header("Name text")]
    [SerializeField]
    Text displayName;

    [Header("Button")]
    [SerializeField]
    Button button;

    private Ingredient ingredient;

    public void SetInfo(Ingredient ingredientArg)
    {
        // Remember this panels ingredient
        ingredient = ingredientArg;

        // Set panel sprite
        image.sprite = ingredient.ItemSprite;
        
        // Set panels ingredient name
        displayName.text = ingredient.DisplayName;
    }

    public void SetInputIngredient()
    {
        GameObject.Find("LevelController").GetComponent<LevelController>().UpdateSelectedInputtersIngredient(ingredient);
    }

    public void CloseIngredientList()
    {
        GameCanvas.Instance.CloseIngredientsList();
    }

    public void SendEvent()
    {
        EventManager.Instance.AddEvent(EventType.Ingredient_Selected);
    }

    public override void Highlight()
    {
        // Highlight ingredient image
        image.color = Color.white;

        // Highlight text
        displayName.color = Color.white;

        // Highlight button image
        button.GetComponent<Image>().color = Color.white;

        // Enable button shadow
        button.GetComponent<Shadow>().enabled = true;

        // Make interactable
        button.interactable = true;
    }

    public override void Dim()
    {
        // Create dimmed colour
        Color dimmed = new Color(0.65f, 0.65f, 0.65f);

        // Dim button image
        button.GetComponent<Image>().color = dimmed;

        // Disable button shadow
        button.GetComponent<Shadow>().enabled = false;

        // Not interactable
        button.interactable = false;

        // Lower alpha of the colour
        dimmed.a = 0.65f;

        // Dim ingredient image
        image.color = dimmed;

        // Dim text
        displayName.color = dimmed;
    }
}
