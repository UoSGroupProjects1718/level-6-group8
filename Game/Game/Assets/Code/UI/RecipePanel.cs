using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePanel : MonoBehaviour
{
    [Header("Potion sprite")]
    [SerializeField]
    Image image;

    [Header("Potion name")]
    [SerializeField]
    Text potionName;

    [Header("Potion ingredient string")]
    [SerializeField]
    Text potionIngredientText;

    public void SetInfo(Potion crafted)
    {
        // Set the sprite
        image.sprite = crafted.ItemSprite;

        // Set title text
        potionName.text = crafted.DisplayName;

        // Build string of ingredient text
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("Ingredients:\n");
        foreach (Item item in crafted.Ingredients)
        {
            sb.Append(string.Format("{0}\n", item.DisplayName));
        }

        // Set the ingredients text
        potionIngredientText.text = sb.ToString();
    }
}
