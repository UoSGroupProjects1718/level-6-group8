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
    [Header("Dull Panel")]
    [SerializeField]
    GameObject dullPanel;

    [Header("Options panel")]
    [SerializeField]
    GameObject optionsPanel;

    [Header("Factory stats display")]
    [SerializeField]
    GameObject factoryUI;
    [SerializeField]
    FactoryPanel factoryPanel;
    
    [Header("Cookbook parent")]
    [SerializeField]
    GameObject cookbookParent;

    [Header("Cookbook scrollable list")]
    [SerializeField]
    GameObject cookbookScrollablePannel;

    [Header("Town hall")]
    [SerializeField]
    GameObject townhallPanel;

    [Header("Player Money")]
    [SerializeField]
    Text primaryMoney;
    [SerializeField]
    Text premiumMoney;

    /// <summary>
    /// At the start of the scene, ensure that
    /// all menus are closed.
    /// </summary>
    void Start()
    {
        CloseAllMenus();
        StartCoroutine(WaitForPlayerLoad());
    }

    IEnumerator WaitForPlayerLoad()
    {
        yield return new WaitForSeconds(GameManager.LoadTime + float.Epsilon);
    }

    /// <summary>
    /// Updates the player money pane in the overworld with the player's primary and premium money count.
    /// </summary>
    /// <param name="player">The player object to get the money values from.</param>
    public void UpdatePlayerStats(uint primary, uint premium)
    {
        primaryMoney.text = Utility.NumberToCommaSeparatedString(primary);
        premiumMoney.text = Utility.NumberToCommaSeparatedString(premium);
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

        // Enables the factory UI
        factoryUI.SetActive(true);

        // Updates the factory UI
        factoryPanel.UpdateUI(factory);
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
        factoryUI.SetActive(false);

        EnableCameraMoveScript();
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// Enables the Town Hall UI Panel
    /// </summary>
    public void DisplayTownHall()
    {
        dullPanel.SetActive(true);

        // First, close any open menus
        CloseAllMenus();

        // Set the UI active
        townhallPanel.SetActive(true);

        // Disable dragging whilst the menu is open
        DisableCameraMoveScript();
    }

    /// <summary>
    /// Disables the Town Hall UI Panel
    /// </summary>
    public void CloseTownHall()
    {
        dullPanel.SetActive(false);

        // Close menus
        CloseAllMenus();

        // Reenable camera drag
        EnableCameraMoveScript();
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
        townhallPanel.SetActive(false);

        EnableCameraMoveScript();
    }

    /// <summary>
    /// Enables the drag script on the camera
    /// </summary>
    private void EnableCameraMoveScript()
    {
        Camera.main.GetComponent<OrthoCameraDrag>().enabled = true;
    }

    /// <summary>
    /// Disables the drag script on the camera
    /// </summary>
    private void DisableCameraMoveScript()
    {
        Camera.main.GetComponent<OrthoCameraDrag>().enabled = false;
    }
}