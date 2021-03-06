﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputterPanel : MonoBehaviour
{
    [SerializeField]
    private Image image;

    /// <summary>
    /// Set the inputters ingredient image
    /// </summary>
    /// <param name="item"></param>
    public void SetImage(Item item)
    {
        if (item == null)
        {
            image.sprite = GameManager.Instance.ResourceManager.TransparentImage;
        }
        else
        {
            image.sprite = item.ItemSprite;
        }
    }
}
