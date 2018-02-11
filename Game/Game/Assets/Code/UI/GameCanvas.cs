using System;
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

    /// <summary>
    ///     Initilization for the UI objects
    ///     @Params Factory
    /// </summary>

    public void BuildUI(Factory factory)
    {
        GetComponent<UI_FactoryEntry>().UpdateUI(factory);
        playing = false;
    }

    public void ToggleDull(bool status)
    {
        dullPanel.SetActive(status);
    }

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

    /// <summary>
    ///     Toggles the Entry Panel
    /// </summary>
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
        LevelController.Instance.EnableDragScript(!EntryPanel.activeSelf);
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

    public void DisplayCookbook()
    {
        cookbookParent.SetActive(true);
        ToggleDull(cookbookParent.activeSelf);
        cookbookScrollablePannel.GetComponent<CookbookScrollableList>().Fill();
        LevelController.Instance.EnableDragScript(false);
    }

    public void ToggleCookbook()
    {
        cookbookParent.SetActive(!cookbookParent.activeSelf);
        ToggleDull(cookbookParent.activeSelf);
        LevelController.Instance.EnableDragScript(!cookbookParent.activeSelf);
    }

    //TODO: maybe overhaul this (John)
    public void LoadIngredientList()
    { 
        ingredientListParent.SetActive(true);
        ToggleDull(ingredientListParent.activeSelf);
        ingredientListPanel.GetComponent<ScrollableList>().Fill();
        LevelController.Instance.EnableDragScript(false);
    }

    public void CloseIngredientList()
    {
        ingredientListParent.SetActive(false);
        ToggleDull(ingredientListParent.activeSelf);
        LevelController.Instance.EnableDragScript(true);
    }

    public void Debug_SetBuildModeText(BuildMode bm)
    {
        debugBuildModeText.text = string.Format("Mode: {0}", bm.ToString());
    }
}
