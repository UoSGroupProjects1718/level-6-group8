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
    private static OverworldCanvas instance = null;
    public static OverworldCanvas Instance { get { return instance; } }

    [Header("Dull Panel")]
    [SerializeField]
    GameObject dullPanel;

    [Header("Options panel")]
    [SerializeField]
    GameObject optionsPanel;

    [Header("Factory stats display")]
    [SerializeField]
    GameObject factoryUI;

    [Header("Town hall")]
    [SerializeField]
    GameObject townhallPanel;

    [Header("Player Money")]
    [SerializeField]
    Text stars;

    Boolean elementOpen;

    /// <summary>
    /// At the start of the scene, ensure that
    /// all menus are closed.
    /// </summary>
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        StartCoroutine(WaitForPlayerLoad());
    }

    void toggleUI()
    {
        elementOpen = !elementOpen;

        // Setup a background dull whilst ui elements are open
        dullPanel.SetActive(elementOpen);
        // Disable dragging whilst the menu is open
        toggleCameraMoveScript(!elementOpen);
    }

    IEnumerator WaitForPlayerLoad()
    {
        yield return new WaitForSeconds(GameManager.LoadTime + float.Epsilon);
        UpdatePlayerStats(GameManager.Instance.Player.Stars);
    }

    /// <summary>
    /// Updates the player money pane in the overworld with the player's primary and premium money count.
    /// </summary>
    /// <param name="player">The player object to get the money values from.</param>
    public void UpdatePlayerStats(uint totalStars)
    {
        stars.text = Utility.NumberToCommaSeparatedString(totalStars);
    }

    /// <summary>
    /// Opens the Options panel.
    /// Disables camera drag script
    /// </summary>
    public void DisplayOptions()
    {
        if (!elementOpen)
        {
            toggleUI();

            optionsPanel.SetActive(true);
        }
    }
    public void CloseOptions()
    {
        toggleUI();

        optionsPanel.SetActive(false);
    }

    /// <summary>
    /// Displays the factory pannel.
    /// If the factory is unlocked, displays factory stats and a button to enter.
    /// Otherwise, displays level to unlock and an "Unlock" button.
    /// </summary>
    /// <param name="factory"></param>
    public void DisplayFactory(Factory factory)
    {
        if (!elementOpen && factory.Unlocked)
        {
            toggleUI();

            // Enables the factory UI
            factoryUI.SetActive(true);

            // Updates the factory UI
            factoryUI.GetComponent<FactoryPanel>().UpdateUI(factory);
        }
    }

    /// <summary>
    /// Closes all factory pannels.
    /// Re-enables the camera drag script.
    /// </summary>
    public void CloseFactoryDisplays()
    {
        toggleUI();

        factoryUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null); // TODO: why is this here?
    }

    /// <summary>
    /// Enables the Town Hall UI Panel
    /// </summary>
    public void DisplayTownHall()
    {
        if (!elementOpen)
        {
            toggleUI();

            // Set the UI active
            townhallPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Disables the Town Hall UI Panel
    /// </summary>
    public void CloseTownHall()
    {
        toggleUI();

        // Set the UI active
        townhallPanel.SetActive(false);
    }

    /// <summary>
    /// Disables the drag script on the camera
    /// </summary>
    private void toggleCameraMoveScript(bool active)
    {
        Camera.main.GetComponent<OrthoCameraDrag>().enabled = active;
    }
}