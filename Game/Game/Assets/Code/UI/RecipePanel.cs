using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePanel : MonoBehaviour
{
    Potion potion;

    [Header("Potion sprite")]
    [SerializeField]
    Image image;

    [Header("Potion name")]
    [SerializeField]
    Text potionName;

    public void SetInfo(Potion crafted)
    {
        potion = crafted;

        // Set the sprite
        image.sprite = crafted.ItemSprite;

        // Set title text
        potionName.text = crafted.DisplayName;

        // Build string of ingredient text
        /*
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("Ingredients:\n");
        foreach (Item item in crafted.Ingredients)
        {
            sb.Append(string.Format("{0}\n", item.DisplayName));
        }

        // Set the ingredients text
        potionIngredientText.text = sb.ToString();
        */
    }

    public void changePanel()
    {
        GameObject panelToLoad = GameObject.Find("Panel_Cookbook");
        CookbookManager cm = panelToLoad.GetComponent<CookbookManager>();
        cm.detailedView(potion);

        // Event
        EventManager.Instance.AddEvent(EventType.Cookbook_PageTurn);
    }
}
