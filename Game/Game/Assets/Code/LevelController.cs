﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    bool running;
    bool canTick;
    int levelWidth, levelHeight;
    int currentlySelected;
    float tickWaitTime;
    public Tile[,] level;
    public GameObject[] Spawnables;
    public List<Machine> Machines = new List<Machine>();

    void Start ()
    {
        running = false;
        canTick = true;
        tickWaitTime = 1.0f;
        currentlySelected = -1;
        DebugLoadLevel();
	}
	
	void Update ()
    {
		if (running)
        {
            if (canTick)
            {
                canTick = false;
                StartCoroutine(TickWait());
                Run();
            }   
        }
	}

    private void Run()
    {
        Debug.Log("Runing...");

        // Tick
        foreach (Machine machine in Machines)
        {
            machine.Tick();
        }

        // Flush
        foreach (Machine machine in Machines)
        {
            machine.Flush();
        }

        // Execute
        foreach (Machine machine in Machines)
        {
            machine.Execute();
        }
    }

    private IEnumerator TickWait()
    {
        canTick = false;
        yield return new WaitForSeconds(tickWaitTime);
        canTick = true;
    }

    public void ToggleRunning()
    {
        if (!running)
        {
            running = true;
        }
        else
        {
            StopRunning();
        }
    }

    private void StopRunning()
    {
        running = false;

        // Clear up all items here
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

        // If the tile on this position owns a tile, return
        if (level[y, x].Machine != null) { return; }

        // Instantiate the machine
        Machine machine = Instantiate(Spawnables[currentlySelected], new Vector3(x, 0, y), Quaternion.identity).GetComponent<Machine>();

        // Set its rotation and parent
        machine.SetDir(Direction.up);
        machine.Parent = level[y, x];

        // Add this machine to the level and to our list of machines
        Machines.Add(machine);

        // Add this machine as the child of the tile
        level[y, x].SetChild(machine);
    }

    public Machine GetNeighbour(int x, int y, Direction facing)
    {
        // Up
        if (facing == Direction.up && y < levelHeight)
        {
            return level[y + 1, x].Machine;
        }
        // Right
        else if (facing == Direction.right && x < levelWidth)
        {
            return level[y, x + 1].Machine;
        }
        // Down
        else if (facing == Direction.down && y > 0)
        {
            return level[y - 1, x].Machine;
        }
        // Left
        else if (facing == Direction.left && x > 0)
        {
            return level[y, x - 1].Machine;
        }

        return null;
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
                Tile tile = Instantiate(Spawnables[0], new Vector3(x, -0.5f, y), Quaternion.identity).GetComponent<Tile>();
                tile.X = x;
                tile.Y = y;

                // Spawn an inputter on top left corner
                if (y == levelHeight-1 && x == levelWidth-1)
                {
                    Machine inputter = Instantiate(Spawnables[2]).GetComponent<Machine>();
                    tile.SetChild(inputter);
                    Machines.Add(inputter);
                    inputter.SetDir(Direction.down);
                }
                // Spawn an inputter on (0, 0);

                level[y, x] = tile;
            }
        }
    }
}
