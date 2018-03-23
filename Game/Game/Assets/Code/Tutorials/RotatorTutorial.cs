using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorTutorial : Tutorial
{
    [Header("Target potion")]
    [SerializeField]
    Potion potion;

    public override void Progress(EventType _event)
    {
        switch (progress)
        {
            case 0:
                // Pan camera
                Camera.main.GetComponent<CameraController>().PanCamera(-0.43f, 2.06f, 0.5f);

                // Add the ingredient to inputter
                LevelController.Instance.LevelFactory.level.grid[5, 4].Machine.GetComponent<Inputter>().SetOutputItem(potion);

                // Send message
                GameCanvas.Instance.DisplayMessage("We need to deliver the potion to two outputs! To do this I've placed a rotator.\n\nPress play to see how it works.");

                // Advance
                progress++;
                break;

            case 1:
                if (_event != EventType.Level_Solved) return;

                // Tutorial complete
                EventManager.Instance.AddEvent(EventType.Tutorial_Solved);

                // Advance
                progress++;
                break;
        }
    }
}
