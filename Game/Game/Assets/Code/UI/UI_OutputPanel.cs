using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OutputPanel : MonoBehaviour
{
    [SerializeField]
    private Image image;

    /// <summary>
    /// Set the inputters ingredient image
    /// </summary>
    /// <param name="item"></param>
    public void SetComplete(bool complete)
    {
        if (complete)
        {
            image.sprite = GameManager.Instance.ResourceManager.OutputComplete;
        }
        else
        {
            image.sprite = GameManager.Instance.ResourceManager.TransparentImage;
        }
    }
}
