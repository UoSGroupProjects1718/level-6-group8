﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [Header("UI Parents")]
    [SerializeField]
    GameObject EntryPanel;

    [SerializeField]
    GameObject selectionListParent;

    [SerializeField]
    GameObject ingredientListParent;

    [SerializeField]
    GameObject ingredientListPanel;

    [SerializeField]
    GameObject cookbookParent;

    [SerializeField]
    GameObject cookbookScrollablePannel;

    [SerializeField]
    GameObject playButton;
    [SerializeField]
    GameObject speedButton;
    [SerializeField]
    GameObject cookbookButton;

    [SerializeField]
    GameObject dullPanel;

    [SerializeField]
    Sprite play;
    [SerializeField]
    Sprite pause;

    [Header("Debug text - build mode")]
    public Text debugBuildModeText;

    private bool playing;

    private GameObject pressed;
    [SerializeField]
    private Color buttonPressedColor;
    [SerializeField]
    private Color buttonDefaultColor;

    private bool isPanelActive;
    private GameObject lastPanel;

    /// <summary>
    ///     Initilization for the UI objects
    ///     @Params Factory
    /// </summary>
    public void BuildUI(Factory factory)
    {
        GetComponent<UI_FactoryEntry>().UpdateUI(factory);
        playing = false;
    }

    /// <summary>
    ///     Toggles the UI used in the level
    /// </summary>
    public void ToggleLevelUI()
    {
        selectionListParent.SetActive(!selectionListParent.activeSelf);
        playButton.SetActive(!playButton.activeSelf);
        speedButton.SetActive(!speedButton.activeSelf);
        cookbookButton.SetActive(!cookbookButton.activeSelf);
    }

    public void onPanelUpdate(GameObject panel)
    {
        isPanelActive = panel.activeSelf;
        lastPanel = panel;

        LevelController.Instance.EnableDragScript(!isPanelActive);
        ToggleDull(isPanelActive);
    }
    private void ToggleDull(bool status)
    {
        dullPanel.SetActive(status);
    }

    /// <summary>
    ///     Button Functions
    /// </summary>
    public void deSelectPreviousButton()
    {
        if(pressed != null)
        {
            pressed.GetComponent<Image>().color = buttonDefaultColor;
        }
    }
    public void buttonPressed(GameObject button)
    {
        deSelectPreviousButton();
        pressed = button;
        pressed.GetComponent<Image>().color = buttonPressedColor;
    }
    public void TogglePlaySprite()
    {
        playing = !playing;
        if (playing)
            playButton.GetComponent<Image>().sprite = pause;
        else
            playButton.GetComponent<Image>().sprite = play;
    }

    /// <summary>
    ///     Toggles the Entry Panel
    /// </summary>
    public void ToggleEntryPanel()
    {
        EntryPanel.SetActive(!EntryPanel.activeSelf);
        onPanelUpdate(EntryPanel);
    }

    /// <summary>
    ///     Cookbook Functions
    /// </summary>
    public void ToggleCookbook()
    {
        if (!isPanelActive)
        {
            cookbookParent.SetActive(!cookbookParent.activeSelf);
            if (cookbookParent.activeSelf)
                loadCookbook();
            onPanelUpdate(cookbookParent);
        }
    }
    private void loadCookbook()
    {
        cookbookScrollablePannel.GetComponent<CookbookScrollableList>().Fill();
    }

    /// <summary>
    ///     Ingredients Functions
    /// </summary>
    public void ToggleIngredientList()
    {
        if (!isPanelActive)
        {
            ingredientListParent.SetActive(!ingredientListParent.activeSelf);
            if (ingredientListParent.activeSelf)
                LoadIngredientList();
            onPanelUpdate(ingredientListParent);
        }
    }
    private void LoadIngredientList()
    {
        ingredientListPanel.GetComponent<ScrollableList>().Fill();
    }
    public void CloseIngredientsList()
    {
        ingredientListParent.SetActive(!ingredientListParent.activeSelf);
        if (ingredientListParent.activeSelf)
            LoadIngredientList();
        onPanelUpdate(ingredientListParent);
    }

    public void Debug_SetBuildModeText(BuildMode bm)
    {
        debugBuildModeText.text = string.Format("Mode: {0}", bm.ToString());
    }
}