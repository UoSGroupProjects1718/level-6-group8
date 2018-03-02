using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionTutorial : Tutorial
{

    public override void Progress(EventType _event)
    {
        int machineX, machineY;

        Debug.Log(string.Format("Progress: {0}", progress));

        // What to do per section
        switch (progress)
        {
                //0: Highlight inputter
            case 0:
                // Disable all UI buttons, we don't need any yet
                GameCanvas.Instance.DisableMachineButtons();

                // Send message
                GameCanvas.Instance.DisplayMessage("Select basil from the list of ingredients and press the Play button. I've placed the conveyers for you.");

                // Dim all tiles and machines
                LevelController.Instance.DimTiles();
                LevelController.Instance.DimMachines();
                LevelController.Instance.DimFactory();

                // Highlight the inputter
                machineX = LevelController.Instance.LevelFactory.DefaultMachines[0].x;
                machineY = LevelController.Instance.LevelFactory.DefaultMachines[0].y;
                LevelController.Instance.LevelFactory.level.grid[machineX, machineY].Machine.Highlight(true);

                // Advance
                progress++;
                break;

                //1: Highlight play button
            case 1:

                // When then ingredient has been selected
                if (_event != EventType.Ingredient_Selected) return;

                // Brighten the factory again
                LevelController.Instance.BrightenTiles();
                LevelController.Instance.BrightenMachines();
                LevelController.Instance.BrightenFactory();

                // Unhighlight the inputter
                machineX = LevelController.Instance.LevelFactory.DefaultMachines[0].x;
                machineY = LevelController.Instance.LevelFactory.DefaultMachines[0].y;
                LevelController.Instance.LevelFactory.level.grid[machineX, machineY].Machine.Highlight(false);

                // Send message
                GameCanvas.Instance.DisplayMessage("Now try hitting the Play button!");

                // Advance
                progress++;
                break;


                //2: Ingredient reaches end
            case 2:

                // When the correct ingredient reaches the end
                if (_event != EventType.Level_Solved) return;

                // Stop running the factories
                LevelController.Instance.ToggleRunning();
                GameCanvas.Instance.TogglePlaySprite();

                // Pan camera, display message
                Camera.main.GetComponent<CameraController>().PanCamera(5, 1.2f, 5);
                GameCanvas.Instance.DisplayMessage("Now, try placing the conveyors yourself!");

                // Enable the conveyor button
                GameCanvas.Instance.EnableMachineButton(Buttons.conveyerButton);

                // Advance
                progress++;
                break;

                //3: Player has to rotate conveyers around a corner
            case 3:

                // When the correct ingredient reaches the end
                if (_event != EventType.Level_Solved) return;

                // Stop running the factories
                LevelController.Instance.ToggleRunning();
                GameCanvas.Instance.TogglePlaySprite();

                // Pan camera, display message
                Camera.main.GetComponent<CameraController>().PanCamera(12, 1.2f, 5);
                GameCanvas.Instance.DisplayMessage("Next, try rotating the conveyers around this corner!");

                // Enable the rotate button
                GameCanvas.Instance.EnableMachineButton(Buttons.rotateButton);

                // Advance
                progress++;
                break;

                //4: Complete
            case 4:
                // When the correct ingredient reaches the end
                if (_event != EventType.Level_Solved) return;

                LevelController.Instance.ToggleRunning();
                GameCanvas.Instance.TogglePlaySprite();

                GameCanvas.Instance.DisplayMessage("Congratulations!\nTutorial complete.");
                break;
        }
    }
}