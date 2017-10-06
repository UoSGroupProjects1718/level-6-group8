﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    build,
    run
}

public class LevelController : MonoBehaviour
{
    // 0 = nothing
    // 1 = tile
    // 2 = input  V
    // 3 = input <
    // 4 = input ^
    // 5 = input >
    // 6 = output

    public int[,,] levels =
    {
        // Level 1
        {
            { 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            { 5, 1, 1, 1, 1, 1, 1, 1, 1, 3},
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0},
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 6},
        },
    };

    int level1X = 6;
    int level1Y = 5;

    Mode gameStatus;
    private Placeable toPlace;
    private bool callTick;
    private float tickTimer;

    public GameObject[] placeables;
    public GameObject[] levelObjects;
    public Square[,] currentLevel;

    public Mode GetGameStatus() { return gameStatus; }

    void Start()
    {
        gameStatus = Mode.build;
        callTick = true;
        tickTimer = 1.0f;
        SpawnLevel(0);
    }

    void Update()
    {
        InputController();

        if (gameStatus == Mode.run)
        {
            ProcessController();
        }

        if (toPlace != null)
        {
            PlaceableController();
        }
    }

    private void ProcessController()
    {
        if (callTick)
        {
            // Call Tick(); on all placeables
            foreach (Placeable placeable in Placeable.allPlaceables)
            {
                placeable.Tick();
            }

            // Call Flush(); on all placeables
            foreach (Placeable placeable in Placeable.allPlaceables)
            {
                placeable.Flush();
            }
        }
    }

    private IEnumerator TickWait()
    {
        callTick = false;
        yield return new WaitForSeconds(tickTimer);
        callTick = true;
    }

    void InputController()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyChild();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (toPlace != null)
            {
                toPlace.GetComponent<BoxCollider2D>().enabled = false;

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Tile"))
                    {
                        toPlace.GetComponent<BoxCollider2D>().enabled = true;
                        hit.collider.GetComponent<Tile>().SetChild(toPlace);
                        toPlace = null;
                        return;
                    }
                }
                else
                {
                    toPlace.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }

    }

    void PlaceableController()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        toPlace.gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, -5);
    }

    public void ToggleMode()
    {
        switch (gameStatus)
        {
            case Mode.run:
                StopRunning("User stopped.", false);
                break;
            case Mode.build:
                DestroyChild();
                gameStatus = Mode.run;
                break;
        }
    }

    public void StopRunning(string reason, bool displayMessageToUser)
    {
        gameStatus = Mode.build;

        // Cleanup all ingredients
        while (Ingredient.allIngredients.Count != 0)
        {
            Destroy(Ingredient.allIngredients[0].gameObject);
            Ingredient.allIngredients.RemoveAt(0);
        }

        // Display reason to user
        if (displayMessageToUser)
        {
            Debug.Log("Process stopped: " + reason);

            //Canvas call
        }
    }

    public void SpawnPlaceable(int index)
    {
        if (index < 0) { return; }
        else  if (index > placeables.Length) { return; }
        else if (gameStatus == Mode.run) { return; }

        DestroyChild();

        toPlace = Instantiate(placeables[index]).GetComponent<Placeable>();
    }

    private void DestroyChild()
    {
        if (toPlace != null)
        {
            Destroy(toPlace.gameObject);
        }
    }

    private void SpawnLevel(int level)
    {
        currentLevel = new Square[level1Y,level1X];

        for (int y = 0; y < level1Y; y++)
        {
            for (int x = 0; x < level1X; x++)
            {
                // 0 = nothing
                // 1 = tile
                // 2 = input  V
                // 3 = input <
                // 4 = input ^
                // 5 = input >
                // 6 = output

                GameObject temp;

                switch (levels[level, y, x])
                {
                    case 0:
                        currentLevel[y, x] = null;
                        continue;
                    case 1:
                        temp = Instantiate(levelObjects[0], new Vector2(x, -y), Quaternion.identity);
                        temp.GetComponent<Tile>().SetXY(x, y);
                        break;
                    case 2:
                        temp = Instantiate(levelObjects[1], new Vector2(x, -y), Quaternion.identity);
                        temp.GetComponent<Placeable>().SetRotation(Direction.down);
                        break;
                    case 3:
                        temp = Instantiate(levelObjects[1], new Vector2(x, -y), Quaternion.identity);
                        temp.GetComponent<Placeable>().SetRotation(Direction.left);
                        break;
                    case 4:
                        temp = Instantiate(levelObjects[1], new Vector2(x, -y), Quaternion.identity);
                        temp.GetComponent<Placeable>().SetRotation(Direction.up);
                        break;
                    case 5:
                        temp = Instantiate(levelObjects[1], new Vector2(x, -y), Quaternion.identity);
                        temp.GetComponent<Placeable>().SetRotation(Direction.right);
                        break;
                    case 6:
                        temp = Instantiate(levelObjects[2], new Vector2(x, -y), Quaternion.identity);
                        break;
                }
            }
        }
    }
}