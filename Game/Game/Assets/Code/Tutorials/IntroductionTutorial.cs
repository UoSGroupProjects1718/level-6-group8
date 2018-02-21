using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionTutorial : Tutorial
{
    [SerializeField]
    GameObject firstInput;

    public override void Progress()
    {
        // Advance onto the next section of the tutorial
        progress++;

        // What to do per section
        switch (progress)
        {
                //1: Introduce the inputter
            case 1:
                // Disable all UI buttons, we don't need any yet
                GameCanvas.Instance.DisableMachineButtons();

                // Send message
                GameCanvas.Instance.DisplayMessage("Select basil from the list of ingredients and press the Play button. I've placed the conveyers for you.");

                // Dim all tiles and machines
                LevelController.Instance.DimTiles();
                LevelController.Instance.DimMachines();
                LevelController.Instance.DimFactory();
                break;

                //2: Player has to place voneyers
            case 2:
                // Stop running the factories
                LevelController.Instance.ToggleRunning();
                GameCanvas.Instance.TogglePlaySprite();

                // Pan camera, display message
                Camera.main.GetComponent<CameraController>().PanCamera(5, 1.2f, 5);
                GameCanvas.Instance.DisplayMessage("Now, try placing the conveyors yourself!");

                // Enable the conveyor button
                GameCanvas.Instance.EnableMachineButton(Buttons.conveyerButton);

                // Brighten the factory again
                LevelController.Instance.BrightenTiles();
                LevelController.Instance.BrightenMachines();
                LevelController.Instance.BrightenFactory();
                break;

                //3: Player has to rotate conveyers around a corner
            case 3:
                // Stop running the factories
                LevelController.Instance.ToggleRunning();
                GameCanvas.Instance.TogglePlaySprite();

                // Pan camera, display message
                Camera.main.GetComponent<CameraController>().PanCamera(12, 1.2f, 5);
                GameCanvas.Instance.DisplayMessage("Next, try rotating the conveyers around this corner!");

                // Enable the rotate button
                GameCanvas.Instance.EnableMachineButton(Buttons.rotateButton);
                break;

                //4: Complete
            case 4:
                LevelController.Instance.ToggleRunning();
                GameCanvas.Instance.TogglePlaySprite();

                GameCanvas.Instance.DisplayMessage("Awesome!\nTutorial complete.");
                break;
        }
    }
}