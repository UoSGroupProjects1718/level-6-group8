using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up,
    right,
    down,
    left
}

public abstract class Placeable : Square
{
    public static List<Placeable> allPlaceables = new List<Placeable>();

    [SerializeField]
    protected Direction dir;

    public int x;
    public int y;

    public Direction getDir() { return dir; }

    void Start()
    { }

    void Update()
    { }

    public void SetPos(int newX, int newY)
    {
        x = newX;
        y = newY;
    }

    public abstract void Tick();
    public abstract void Flush();

    public abstract void GiveIngredient(Ingredient newIngredient);

    private void Rotate()
    {
        switch (dir)
        {
            case Direction.up:
                dir = Direction.right;
                break;
            case Direction.right:
                dir = Direction.down;
                break;
            case Direction.down:
                dir = Direction.left;
                break;
            case Direction.left:
                dir = Direction.up;
                break;
        }
        transform.Rotate(new Vector3(0, 0, 1), -90);
    }

    public void SetRotation(Direction newDir)
    {
        dir = newDir;

        switch (newDir)
        {
            case Direction.up:
                transform.eulerAngles = new Vector3(0, 0, 180);
                Debug.Log("Setting direction up");
                break;
            case Direction.right:
                transform.eulerAngles = new Vector3(0, 0, 90);
                Debug.Log("Setting direction right");
                break;
            case Direction.down:
                transform.eulerAngles = new Vector3(0, 0, 0);
                Debug.Log("Setting direction down");
                break;
            case Direction.left:
                transform.eulerAngles = new Vector3(0, 0, -90);
                Debug.Log("Setting direction left");
                break;
        }
    }

    private void OnMouseOver()
    {
        // Right mouse click
        if (Input.GetMouseButtonDown(1))
        {
            // Return if the machines are running
            if (GameObject.Find("LevelController").GetComponent<LevelController>().GetGameStatus() == Mode.run)
            {
                return;
            }

            Rotate();
        }
    }
}