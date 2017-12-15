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
            machine.gameObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            machine.SetDir(Direction.up);
            machine.Parent = this;
        }
    }

    #region PcControls

    // Mouse controls, specific to PC
    void OnMouseOver()
    {
#if UNITY_EDITOR
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            LeftClick();
        }
        
        // Right click
        else if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }
#endif
    }

    private void LeftClick()
    {
        if (!active) { return; }

        LevelController.Instance.SpawnOn(x, y);
    }

    private void RightClick()
    {
        ToggleActive();
    }

    #endregion

    #region MobileControls

    // This will detect when a user taps on the tile
    void OnMouseDown()
    {
        // Return unless we're active
        if (!active) { return; }

        // Check that the user is in an appropriate build mode
        // (What do they have selected?)
        switch (LevelController.Instance.BuildStatus)
        {
            case BuildMode.brewer:
            case BuildMode.conveyer:
            case BuildMode.grinder:
                LevelController.Instance.SpawnOn(x, y);
                break;
        }
    }

    #endregion

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