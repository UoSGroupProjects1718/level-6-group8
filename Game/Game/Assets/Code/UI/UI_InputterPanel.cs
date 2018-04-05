﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputterPanel : MonoBehaviour
{
    [SerializeField]
    private Image ingredientImage;

    /// <summary>
    /// Set the inputters ingredient image
    /// </summary>
    /// <param name="item"></param>
    public void SetIngredientImage(Item item)
    {
        if (item == null)
        {
            ingredientImage.sprite = GameManager.Instance.ResourceManager.TransparentImage;
        }
        else
        {
            ingredientImage.sprite = item.ItemSprite;
        }
    }
}
