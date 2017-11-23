using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    private bool active = true;
    [SerializeField]
    private Machine machine;

    public int X { get { return x; } set { x = value; } }
    public int Y { get { return y; } set { y = value; } }
    public bool ActiveTile { get { return active; } }
    public Machine Machine { get { return machine; } }

    public void SetChild(Machine newChild)
    {
        machine = newChild;

        if (newChild != null)
        {
            machine.gameObject.transform.position = new Vector3(transform.position.x, -0.3f, transform.position.z);
            machine.transform.eulerAngles = new Vector3(60, 45, 0);
            machine.SetDir(Direction.up);
            machine.Parent = this;
        }
    }

    public virtual void OnClick()
    {
        LevelController lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        lc.SpawnOn(x, y);
    }

    public void LeftClick()
    {
        if (!active) { return; }
        OnClick();
    }

    public void RightClick()
    {
        ToggleActive();
    }

    public void SetActiveStatus(bool foo)
    {
        if (foo)
        {
            Activate();
        }
        else
        {
            DeActivate();
        }
    }

    void ToggleActive()
    {
        if (active)
        {

            DeActivate();
        }
        else
        {
            Activate();
        }
    }


    private void Activate()
    {
        // Activate
        active = true;
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    private void DeActivate()
    {
        // Deactivate
        active = false;
        gameObject.GetComponent<Renderer>().material.color = Color.black;
    }

}
