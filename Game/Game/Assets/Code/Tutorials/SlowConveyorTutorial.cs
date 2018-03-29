using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowConveyorTutorial : Tutorial
{
    public override void Progress(EventType _event)
    {
        switch (progress)
        {
            case 0:

                // Pan camera
                Camera.main.GetComponent<CameraController>().PanCamera(-0.75f, 2.3f, 0.5f);

                // Disable buttons
                GameCanvas.Instance.DisableMachineButtons();

                // Enable the ones we want
                GameCanvas.Instance.EnableMachineButton(Buttons.conveyerButton);
                GameCanvas.Instance.EnableMachineButton(Buttons.grinderButton);
                GameCanvas.Instance.EnableMachineButton(Buttons.brewerButton);
                GameCanvas.Instance.EnableMachineButton(Buttons.slowConveyerButton);

                GameCanvas.Instance.EnableMachineButton(Buttons.rotateButton);
                GameCanvas.Instance.EnableMachineButton(Buttons.deleteButton);

                // Send message
                GameCanvas.Instance.DisplayMessage("These inputs are not synced up, I have used a slow conveyor to fix the timing!\n\nPress play to see how it works.");

                // Add the ingredients to inputters
                LevelController.Instance.LevelFactory.level.grid[1, 7].Machine.GetComponent<Inputter>().SetOutputItem(
                    LevelController.Instance.LevelFactory.Targets[0].GetComponent<Potion>().Ingredients[0]);
                LevelController.Instance.LevelFactory.level.grid[3, 6].Machine.GetComponent<Inputter>().SetOutputItem(
                    LevelController.Instance.LevelFactory.Targets[0].GetComponent<Potion>().Ingredients[1]);


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
