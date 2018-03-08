using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Buttons
{
    ovenButton,
    conveyerButton,
    grinderButton,
    brewerButton,
    rotateButton,
    deleteButton,
    slowConveyerButton,
    RotateConveyerButton
}

public class GameCanvas : MonoBehaviour
{
    // Singleton
    private static GameCanvas instance;
    public static GameCanvas Instance { get { return instance; } }

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

    [Header("Final score screen")]
    [SerializeField]
    UI_ScoreScreen scoreScreen;

    [Header("Production line buttons")]
    [SerializeField]
    GameObject playButton;
    [SerializeField]
    GameObject speedButton;

    [Header("Cookbook button")]
    [SerializeField]
    GameObject cookbookButton;

    [Header("Selection Buttons (Left panel)")]
    [SerializeField]
    GameObject ovenButton;
    [SerializeField]
    GameObject conveyerButton;
    [SerializeField]
    GameObject grinderButton;
    [SerializeField]
    GameObject brewerButton;
    [SerializeField]
    GameObject rotateButton;
    [SerializeField]
    GameObject deleteButton;
    [SerializeField]
    GameObject slowConveyerButton;
    [SerializeField]
    GameObject RotateConveyerButton;

    [Header("Dull")]
    [SerializeField]
    GameObject dullPanel;

    [Header("Sprites")]
    [SerializeField]
    Sprite play;
    [SerializeField]
    Sprite pause;

    [Header("Mayor display message")]
    [SerializeField]
    GameObject messagePanel;
    [SerializeField]
    Text messageText;

    [Header("Debug text - build mode")]
    public Text debugBuildModeText;



    [SerializeField]
    private Color buttonPressedColor;
    [SerializeField]
    private Color buttonDefaultColor;

    // Private
    private bool isPanelActive;  
    private bool playing;
    private GameObject pressed;
    private GameObject lastPanel;
    private Dictionary<Buttons, GameObject> machineButtons;

    private void Awake()
    {
        // Initialiaze the Singleton
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        // Pushback machine buttons
        InitButtonDictionary();
    }

    private void Start()
    {

    }

    /// <summary>
    /// Initializes the private dictionary we keep to store each 
    /// button against an enum.
    /// </summary>
    private void InitButtonDictionary()
    {
        machineButtons = new Dictionary<Buttons, GameObject>();

        machineButtons.Add(Buttons.brewerButton, brewerButton);
        machineButtons.Add(Buttons.conveyerButton, conveyerButton);
        machineButtons.Add(Buttons.deleteButton, deleteButton);
        machineButtons.Add(Buttons.grinderButton, grinderButton);
        machineButtons.Add(Buttons.ovenButton, ovenButton);
        machineButtons.Add(Buttons.rotateButton, rotateButton);
        machineButtons.Add(Buttons.RotateConveyerButton, RotateConveyerButton);
        machineButtons.Add(Buttons.slowConveyerButton, slowConveyerButton);
    }

    /// <summary>
    /// Disables each Machine button in the left panel
    /// </summary>
    public void DisableMachineButtons()
    {
        foreach (var pair in machineButtons)
        {
            pair.Value.SetActive(false);
        }
    }

    /// <summary>
    /// Enables a given Machine button
    /// </summary>
    public void EnableMachineButton(Buttons key)
    {
        machineButtons[key].SetActive(true);
    }

    /// <summary>
    /// Displays a given message to the screen in a text box
    /// </summary>
    /// <param name="message"></param>
    public void DisplayMessage(string message)
    {
        messagePanel.SetActive(true);
        messageText.text = message;
    }

    /// <summary>
    /// Closes the Major Display Pannel
    /// </summary>
    public void CloseMessage()
    {
        messagePanel.SetActive(false);
    }

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

        // Event
        EventManager.Instance.AddEvent(EventType.Machine_Selected);
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
    public void CloseCookbook()
    {
        cookbookParent.SetActive(!cookbookParent.activeSelf);
        onPanelUpdate(cookbookParent);
        EventManager.Instance.AddEvent(EventType.Cookbook_PageTurn);
    }
    private void loadCookbook()
    {
        cookbookScrollablePannel.GetComponent<CookbookScrollableList>().Fill();
        EventManager.Instance.AddEvent(EventType.Cookbook_PageTurn);
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

    public void EnableScoreScreen(int score, int ticks)
    {
        // Dull background
        ToggleDull(true);

        // Set score screen as active active
        scoreScreen.gameObject.SetActive(true);

        // Update the values
        scoreScreen.SetScore(score, ticks);
    }

    public void CloseScoreScreen()
    {
        // Set it inactive
        scoreScreen.gameObject.SetActive(false);
    }

    public void Debug_SetBuildModeText(BuildMode bm)
    {
        debugBuildModeText.text = string.Format("Mode: {0}", bm.ToString());
    }
}