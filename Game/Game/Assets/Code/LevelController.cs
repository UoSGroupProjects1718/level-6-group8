using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    bool running;
    int levelWidth, levelHeight;
    int currentlySelected;
    public Tile[,] level;
    public GameObject[] Spawnables;

	void Start ()
    {
        running = false;
        DebugLoadLevel();
	}
	
	void Update ()
    {
		if (running)
        {
            Run();
        }
	}

    private void Run()
    {
        Debug.Log("Runing...");

        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                level[y, x].Tick();
            }
        }

        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                level[y, x].Flush();
            }
        }

        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                level[y, x].Execute();
            }
        }
    }

    public void ToggleRunning()
    {
        running = !running;
    }

    public void SetCurrentlySelected(int i)
    {
        // If the same button is pressed again,
        if (currentlySelected == i)
        {
            // Unselect the currently selected
            currentlySelected = -1;
        }
        // Otherwise
        else
        {
            // This is our selected item
            currentlySelected = i;
        }
    }

    public void SpawnOn(int x, int y)
    {
        // Return checks
        if (currentlySelected == -1) { return; }

        // Destroy the current object on this position
        Destroy(level[y, x].gameObject);


        // Replace it with the new object
        Tile tile = Instantiate(Spawnables[currentlySelected], new Vector3(x, 0, y), Quaternion.identity).GetComponent<Tile>();
        tile.X = x;
        tile.Y = y;
        level[y, x] = tile;
    }

    //! Instantiates test level for debugging purposes
    private void DebugLoadLevel()
    {
        levelWidth = 10;
        levelHeight = 10;

        level = new Tile[levelWidth, levelHeight];

        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                Tile tile = Instantiate(Spawnables[0], new Vector3(x, 0, y), Quaternion.identity).GetComponent<Tile>();
                tile.X = x;
                tile.Y = y;

                level[y, x] = tile;
            }
        }
    }
}
