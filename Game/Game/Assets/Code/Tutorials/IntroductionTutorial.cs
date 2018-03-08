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
                // Move camera
                Camera.main.GetComponent<CameraController>().PanCamera(-1, 1.5f, 0.5f);

                // Disable all UI buttons, we don't need any yet
                GameCanvas.Instance.DisableMachineButtons();

                // Send message
                GameCanvas.Instance.DisplayMessage("First, let's select an ingredient!\nSelect Basil from the ingredient list - tap on the highlighted inputter to open the list.");

                // Dim all tiles and machines
                LevelController.Instance.DimTiles();
                LevelController.Instance.DimMachines();
                LevelController.Instance.DimFactory();

                // Highlight the inputter
                machineX = LevelController.Instance.LevelFactory.DefaultMachines[0].x;
                machineY = LevelController.Instance.LevelFactory.DefaultMachines[0].y;
                LevelController.Instance.LevelFactory.level.grid[machineX, machineY].Machine.Outline(true);

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
                LevelController.Instance.LevelFactory.level.grid[machineX, machineY].Machine.Outline(false);

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
                GameCanvas.Instance.DisplayMessage("Now try it yourself! You can use the buttons on the left to place and delete machines.\n\nDon't forget to select the Basil ingredient again!");

                // Darken the tiles
                LevelController.Instance.DimTiles();

                // Highlight the correct tiles
                LevelController.Instance.LevelFactory.level.grid[8, 2].Brighten();
                LevelController.Instance.LevelFactory.level.grid[8, 3].Brighten();
                LevelController.Instance.LevelFactory.level.grid[8, 4].Brighten();

                // Enable the conveyor button
                GameCanvas.Instance.EnableMachineButton(Buttons.conveyerButton);
                GameCanvas.Instance.EnableMachineButton(Buttons.deleteButton);

                // Advance
                progress++;
                break;

                //3: Player has to rotate conveyers around a corner
            case 3:

                // When the correct ingredient reaches the end
                if (_event != EventType.Level_Solved) return;

                // Rebrighten the tiles
                LevelController.Instance.BrightenTiles();

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