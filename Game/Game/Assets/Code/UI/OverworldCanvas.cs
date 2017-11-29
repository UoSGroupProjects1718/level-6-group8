using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for controlling the Canvas 
/// in the Overworld scene.
/// </summary>
public class OverworldCanvas : MonoBehaviour
{
    [Header("Options panel")]
    [SerializeField]
    GameObject optionsPanel;

    [Header("Factory stats display")]
    [SerializeField]
    GameObject factoryStatsPanel;
    [SerializeField]
    RawImage factorySprite;
    [SerializeField]
    Text factoryName;
    [SerializeField]
    Text factoryMainText;
    [SerializeField]
    Text factoryGoalName;
    [SerializeField]
    Image[] factoryStars;
    [SerializeField]
    Slider factoryStockpile;



    [SerializeField]
    private Sprite StarFilled;
    [SerializeField]
    private Sprite StarEmpty;


    [Header("Factory unlock panel")]
    [SerializeField]
    GameObject factoryPurchasePanel;
    [SerializeField]
    RawImage factoryPurchaseSprite;
    [SerializeField]
    Text factoryPurchaseName;
    [SerializeField]
    Text factoryPurchaseCost;


    [Header("Cookbook parent")]
    [SerializeField]
    GameObject cookbookParent;

    [Header("Cookbook scrollable list")]
    [SerializeField]
    GameObject cookbookScrollablePannel;

    /// <summary>
    /// At the start of the scene, ensure that
    /// all menus are closed.
    /// </summary>
    void Start()
    {
        CloseAllMenus();
    }

    public void UpdatePlayerStats(Player player)
    {
        //playerMoney.text = player.Money;

        // Etc...
    }

    /// <summary>
    /// Opens the Options panel.
    /// Disables camera drag script
    /// </summary>
    public void DisplayOptions()
    {
        optionsPanel.SetActive(true);

        // Disable dragging whilst the menu is open
        DisableCameraMoveScript();
    }

    /// <summary>
    /// Displays the factory pannel.
    /// If the factory is unlocked, displays factory stats and a button to enter.
    /// Otherwise, displays level to unlock and an "Unlock" button.
    /// </summary>
    /// <param name="factory"></param>
    public void DisplayFactory(Factory factory)
    {
        // Close all current open menus before opening a new menu/interface
        CloseFactoryDisplays();

        // Disable dragging whilst the menu is open
        DisableCameraMoveScript();

        // Check which pannel to open
        if (factory.Unlocked)
        {
            factoryStatsPanel.SetActive(true);

            factoryName.text = factory.FactoryName;
            factorySprite.texture = factory.FactorySprite;
            factoryGoalName.text = factory.Potion.DisplayName;

            if (factory.Solved)
            {
                factoryMainText.text = "Score: " + factory.Score;
                fillfactoryStars(factory.Stars);
               // factoryStockpile.maxValue = factory.StockpileLimit;
            } else {
                factoryMainText.text = "Complete the level to recieve a score!";
                fillfactoryStars(0);
            }

            //factoryStockpile.value;
        }
        /*
        else
        {
            factoryPurchasePanel.SetActive(true);

            factoryPurchaseSprite.texture = factory.FactorySprite;
            factoryPurchaseName.text = factory.FactoryName;
            factoryPurchaseCost.text = string.Format("{0} Stars Needed", factory.starsToUnlock);
        }
        */
    }

    private void fillfactoryStars(uint stars)
    {
        for(int i = 0; i < factoryStars.Length; i++)
        {
            if (i < stars) {
                factoryStars[i].color = Color.yellow;
                factoryStars[i].sprite = StarFilled;
            } else
            {
                factoryStars[i].color = Color.gray;
                factoryStars[i].sprite = StarEmpty;
            }
        }
    }

    /// <summary>
    /// Opens the Cookbook pannel.
    /// Calls the scrollable list to create all recipes.
    /// Disables cameras drag script.
    /// </summary>
    public void DisplayCookbook()
    {
        // First, close any open menus
        CloseAllMenus();

        // Set the cookbook parent object active
        cookbookParent.SetActive(true);
        cookbookScrollablePannel.GetComponent<CookbookScrollableList>().Fill();

        // Disable dragging whilst the menu is open
        DisableCameraMoveScript();
    }

    /// <summary>
    /// Closes all factory pannels.
    /// Re-enables the camera drag script.
    /// </summary>
    public void CloseFactoryDisplays()
    {
        factoryStatsPanel.SetActive(false);
        factoryPurchasePanel.SetActive(false);

        EnableCameraMoveScript();
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// Closes all menus.
    /// Re-Enables the camera drag script.
    /// </summary>
    public void CloseAllMenus()
    {
        CloseFactoryDisplays();
        optionsPanel.SetActive(false);
        cookbookParent.SetActive(false);

        EnableCameraMoveScript();
    }

    /// <summary>
    /// Enables the drag script on the camera
    /// </summary>
    private void EnableCameraMoveScript()
    {
        Camera.main.GetComponent<MoveCamera>().enabled = true;
    }

    /// <summary>
    /// Disables the drag script on the camera
    /// </summary>
    private void DisableCameraMoveScript()
    {
        Camera.main.GetComponent<MoveCamera>().enabled = false;
    }
}