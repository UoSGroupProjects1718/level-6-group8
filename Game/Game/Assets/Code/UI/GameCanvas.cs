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

    [Header("Debug text - build mode")]
    public Text debugBuildModeText;

    /// <summary>
    ///     Initilization for the UI objects
    ///     @Params Factory
    /// </summary>

    public void BuildUI(Factory factory)
    {
        this.GetComponent<UI_FactoryEntry>().UpdateUI(factory);
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
    ///     Toggles the Selection Panel
    /// </summary>
    public void ToggleSelectionPanel()
    {
        selectionListParent.SetActive(!selectionListParent.activeSelf);
    }

    //TODO: maybe overhaul this (John)
    public void LoadIngredientList()
    {
        ingredientListParent.SetActive(true);
        ingredientListPanel.GetComponent<ScrollableList>().Fill();
        LevelController.Instance.EnableDragScript(false);
    }

    public void CloseIngredientList()
    {
        ingredientListParent.SetActive(false);
        LevelController.Instance.EnableDragScript(true);
    }

    public void Debug_SetBuildModeText(BuildMode bm)
    {
        debugBuildModeText.text = string.Format("Mode: {0}", bm.ToString());
    }
}
