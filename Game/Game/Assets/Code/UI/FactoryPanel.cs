using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryPanel : MonoBehaviour {

    [Header("Objects")]
    [SerializeField]
    GameObject dull;

    [Header("UI Elements")]
    [SerializeField]
    RawImage factorySprite;
    [SerializeField]
    Text factoryName;
    [SerializeField]
    Text factoryMainText;
    [SerializeField]
    Image[] factoryStars;
    [SerializeField]
    Slider factoryStockpile;
    [SerializeField]
    Slider factoryIncome;
    [SerializeField]
    Slider factoryCost;
    public Image HighscorePanel; 

    [Header("Asset Reference")]
    [SerializeField]
    private Sprite StarFilled;
    [SerializeField]
    private Sprite StarEmpty;

    /// <summary>
    /// Displays the factory pannel.
    /// If the factory is unlocked, displays factory stats and a button to enter.
    /// Otherwise, displays level to unlock and an "Unlock" button.
    /// </summary>
    /// <param name="factory"></param>
    public void UpdateUI(Factory factory)
    {
        // Disable dragging whilst the menu is open
        DisableCameraMoveScript();

        // Check which pannel to open
        if (factory.Unlocked)
        {
            toggleDim(true);

            factoryName.text = factory.FactoryName;
            factorySprite.texture = factory.FactorySprite;

            if (factory.Solved)
            {
                factoryMainText.text = "Score: " + factory.Score;
                fillfactoryStars(factory.Stars);
                factoryStockpile.maxValue = factory.StockpileLimit;
                factoryStockpile.value = factory.stockpile.ItemCount;

                //TODO: we need score thresholds stored in the factory data
                factoryIncome.value = factory.Target.Cost / factory.TicksToSolve;
                factoryIncome.maxValue = 30; // factory.maxTicksToSolve, predicted maximum ticks to solve

                factoryCost.value = factory.TotalMachineCost; //factory.totalmachineCost;
                factoryCost.maxValue = 100; // factory.maxTotalmachineCost, predicted maximum ticks to solve
            }
            else
            {
                factoryMainText.text = "Complete the level to recieve a score!";
                fillfactoryStars(0);
                factoryStockpile.value = 0;
            }
        }
    }

    public void toggleDim(bool active)
    {
        dull.SetActive(active);
    }

    private void fillfactoryStars(uint stars)
    {
        for (int i = 0; i < factoryStars.Length; i++)
        {
            if (i < stars)
            {
                factoryStars[i].color = Color.yellow;
                factoryStars[i].sprite = StarFilled;
            }
            else
            {
                factoryStars[i].color = Color.gray;
                factoryStars[i].sprite = StarEmpty;
            }
        }
    }

    public void ToggleHighscorePanel()
    {
        HighscorePanel.gameObject.SetActive(!HighscorePanel.gameObject.activeSelf);
    }

    /// <summary>
    /// Disables the drag script on the camera
    /// </summary>
    private void DisableCameraMoveScript()
    {
        Camera.main.GetComponent<OrthoCameraDrag>().enabled = false;
    }
}