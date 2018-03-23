using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinesTutorial : Tutorial
{
    public override void Progress(EventType _event)
    {
        switch (progress)
        {
            case 0:

                // Pan camera
                Camera.main.GetComponent<CameraController>().PanCamera(0.35f, 1.6f, 0.5f);

                // Disable buttons
                GameCanvas.Instance.DisableMachineButtons();

                // Enable the ones we want
                GameCanvas.Instance.EnableMachineButton(Buttons.conveyerButton);
                GameCanvas.Instance.EnableMachineButton(Buttons.grinderButton);
                GameCanvas.Instance.EnableMachineButton(Buttons.brewerButton);

                // Send message
                GameCanvas.Instance.DisplayMessage("Time to make a health potion!\nTake a look in the spell book to find out the ingredients and then select them on the inputters!");

                // Progress
                progress++;
                break;

            case 1:

                if (_event != EventType.Level_Solved) return;

                // Solved tutorial
                EventManager.Instance.AddEvent(EventType.Tutorial_Solved);

                // Progress
                progress++;

                break;
        }
    }
}
