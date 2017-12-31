﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FactoryEntry : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    Text text_factoryName;
    [SerializeField]
    Text text_factoryPotionName;
    [SerializeField]
    Text text_factoryPotion;
    [SerializeField]
    Text text_factoryHint;
    [SerializeField]
    Image img_potion;

    /// <summary>
    /// Displays a entry splash screen for the factory.
    /// 
    /// 
    /// </summary>
    /// <param name="factory"></param>
    public void UpdateUI(Factory factory)
    {
        text_factoryName.text = factory.FactoryName;
        text_factoryPotionName.text = factory.Potion.DisplayName;
        text_factoryPotion.text = factory.Potion.DisplayName;

        // TODO: Random hint on load
        text_factoryHint.text = "Hint: The recipe book contains vitial instructions on potion creation.";

        img_potion.sprite = factory.Potion.ItemSprite;
    }
}