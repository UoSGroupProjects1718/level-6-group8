using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientScrollableList : ScrollableList
{
    public override void Fill()
    {
        if (loaded == true) { return; }
        loaded = true;

        int itemCount = GameManager.Instance.Ingredients.Length;

        RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform>();
        RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();

        // Calculate the width and height of each child
        float width = containerRectTransform.rect.width / columnCount;
        float ratio = width / rowRectTransform.rect.width;
        float height = rowRectTransform.rect.height * ratio;
        int rowCount = itemCount / columnCount;
        if (rowCount != 0 && itemCount % rowCount > 0) { rowCount++; }

        // Adjust the height of the container so that it fits all children
        float scrollHeight = height * rowCount;
        containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
        containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);

        int j = 0;
        for (int i = 0; i < itemCount; i++)
        {
            // This is favourable over a double for loop because itemCount may not fit perfectly into
            if (i % columnCount == 0) { j++; }

            // Create a new item, name it, set this as parent
            GameObject newItem = Instantiate(itemPrefab) as GameObject;
            // children.Add(newItem);
            newItem.name = string.Format("{0} Item at: {1},{2}", gameObject.name, i, j);
            newItem.transform.SetParent(gameObject.transform);

            // Move and size this new item
            RectTransform rectTransform = newItem.GetComponent<RectTransform>();

            float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
            float y = containerRectTransform.rect.height / 2 - height * j;
            rectTransform.offsetMin = new Vector2(x, y);

            x = rectTransform.offsetMin.x + width;
            y = rectTransform.offsetMin.y + height;
            rectTransform.offsetMax = new Vector2(x, y);

            // Finally, update information
            newItem.GetComponent<IngredientPanel>().SetInfo(GameManager.Instance.Ingredients[i].GetComponent<Ingredient>());
        }
    }
}
