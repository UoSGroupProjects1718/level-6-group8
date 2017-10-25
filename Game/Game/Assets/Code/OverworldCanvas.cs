using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldCanvas : MonoBehaviour
{

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

    void Start()
    {
        factoryStatsPanel.SetActive(false);
    }

    void Update()
    {

    }

    public void UpdatePlayerStats(/*Player player*/)
    {
        //playerMoney.text = player.Money;

        // Etc...
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

    public void CloseFactoryDisplays()
    {
        factoryStatsPanel.SetActive(false);
        factoryPurchasePanel.SetActive(false);
    }

    private void CloseAllMenus()
    {
        factoryStatsPanel.SetActive(false);
        factoryPurchasePanel.SetActive(false);
    }
}