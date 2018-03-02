using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PotionPanel : MonoBehaviour
{
    [Header("Left Page")]
    [SerializeField]
    Image image_largePotion;
    [SerializeField]
    Text text_potionName;

    [Header("Right Page")]
    [SerializeField]
    GameObject ingredientParent;
    [SerializeField]
    GameObject machineParent;
    [SerializeField]
    GameObject image_ingredientPrefab;


    //TODO this is probably not clean
    public void fillPanel(Potion pot)
    {
        image_largePotion.sprite = pot.ItemSprite;
        text_potionName.text = pot.DisplayName;

        RectTransform containerRectTransform = ingredientParent.GetComponent<RectTransform>();
        RectTransform rowRectTransform = image_ingredientPrefab.GetComponent<RectTransform>();

        float width = containerRectTransform.rect.width / pot.Ingredients.Count;
        float ratio = width / rowRectTransform.rect.width;
        float height = rowRectTransform.rect.height * ratio;

        int i = 0;
        foreach (Item ingredient in pot.Ingredients)
        {
            // Create a new item, name it, set this as parent
            GameObject newItem = Instantiate(image_ingredientPrefab) as GameObject;
            newItem.name = string.Format("{0}", ingredient.DisplayName);
            newItem.transform.SetParent(ingredientParent.transform);

            Image ingredientImg = newItem.GetComponent<Image>();
            ingredientImg.sprite = ingredient.ItemSprite;

            RectTransform rectTransform = newItem.GetComponent<RectTransform>();

            float x = -containerRectTransform.rect.width / 2 + (width * i);
            float y = containerRectTransform.rect.height / 2 - height;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x + width;
            y = rectTransform.offsetMin.y + height;
            rectTransform.offsetMax = new Vector2(x, y);

            newItem.transform.localScale = new Vector3(0.85f, 0.9f, 1f);
            i++;
        }

        containerRectTransform = machineParent.GetComponent<RectTransform>();
        i = 0;
        foreach (Machine machine in pot.Machines)
        {
            // Create a new item, name it, set this as parent
            GameObject newItem = Instantiate(image_ingredientPrefab) as GameObject;
            newItem.name = string.Format("{0}", machine.name);
            newItem.transform.SetParent(machineParent.transform);

            Image ingredientImg = newItem.GetComponent<Image>();
            ingredientImg.sprite = machine.Sprite;

            RectTransform rectTransform = newItem.GetComponent<RectTransform>();

            float x = -containerRectTransform.rect.width / 2 + (width * i);
            float y = containerRectTransform.rect.height / 2 - height;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x + width;
            y = rectTransform.offsetMin.y + height;
            rectTransform.offsetMax = new Vector2(x, y);

            newItem.transform.localScale = new Vector3(0.85f, 0.9f, 1f);
            i++;
        }
    }

    public void clearPanel()
    {
        foreach (Transform child in ingredientParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
