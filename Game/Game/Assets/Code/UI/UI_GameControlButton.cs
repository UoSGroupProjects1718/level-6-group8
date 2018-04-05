using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The UI Buttons during the Level such as Conveyor,
/// Brewer, Rotate and Delete buttons.
/// </summary>
public class UI_GameControlButton : DimmablePanel
{
    [Header("Machine/control sprite")]
    [SerializeField]
    Image image;

    [Header("Button text")]
    [SerializeField]
    Text buttonText;
    
    // String to remember the original text of the button
    private string originalText;

    // Sprite to remember the original Sprite of the button
    private Sprite originalSprite;

    // Toggle component
    private Toggle button;

    void Start()
    {
        // Remember the buttons text
        originalText = buttonText.text;

        // Remember the original sprite
        originalSprite = image.sprite;

        button = GetComponent<Toggle>();
    }


    public override void Highlight()
    {
        // Re-add the image
        image.sprite = originalSprite;

        // Re-add the text
        buttonText.text = originalText;

        // Highlight button image
        button.GetComponent<Image>().color = Color.white;

        // Enable button shadow
        button.GetComponent<Shadow>().enabled = true;

        // Make interactable
        button.interactable = true;
    }

    public override void Dim()
    {
        // Create dimmed colour
        Color dimmed = new Color(0.65f, 0.65f, 0.65f);

        // Dim the button
        button.GetComponent<Image>().color = dimmed;

        // Disable button shadow
        button.GetComponent<Shadow>().enabled = false;

        // Make it non-interactable
        button.interactable = false;

        // Remove the image
        image.sprite = GameManager.Instance.ResourceManager.TransparentImage;

        // Remove the text
        buttonText.text = "";
    }


}
