using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    Image factorySprite;
    [SerializeField]
    Text factoryName;

    [Header("Factory purchase panel")]
    [SerializeField]
    GameObject factoryPurchasePanel;
    [SerializeField]
    Image factoryPurchaseSprite;
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

    void Start()
    {
        CloseAllMenus();
    }


    public void UpdatePlayerStats(/*Player player*/)
    {
        //playerMoney.text = player.Money;

        // Etc...
    }

    public void DisplayOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void DisplayFactory(Factory factory)
    {
        // Close all current open menus before opening a new menu/interface
        CloseFactoryDisplays();

        if (factory.IsUnlocked)
        {
            factoryStatsPanel.SetActive(true);

            factorySprite.sprite = factory.FactorySprite;
            factoryName.text = factory.FactoryName;
        }
        else
        {
            factoryPurchasePanel.SetActive(true);

            factoryPurchaseSprite.sprite = factory.FactorySprite;
            factoryPurchaseName.text = factory.FactoryName;
            factoryPurchaseCost.text = string.Format("Unlocked at level {0}", factory.LevelToUnlock);
        }
    }

    public void DisplayCookbook()
    {
        // First, close any open menus
        CloseAllMenus();

        // Set the cookbook parent object active
        cookbookParent.SetActive(true);
        cookbookScrollablePannel.GetComponent<CookbookScrollableList>().Fill();
    }

    public void CloseCookbook()
    {
        CloseAllMenus();
    }

    public void CloseFactoryDisplays()
    {
        factoryStatsPanel.SetActive(false);
        factoryPurchasePanel.SetActive(false);
    }

    public void CloseAllMenus()
    {
        CloseFactoryDisplays();
        optionsPanel.SetActive(false);
        cookbookParent.SetActive(false);
    }
}