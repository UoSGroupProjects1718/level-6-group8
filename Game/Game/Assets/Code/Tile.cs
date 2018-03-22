using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : DimmableObject
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

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            // Raycast to see if we hit
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.transform == this.gameObject.transform)
                {
                    OnTouch(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                }
            }

        }
    }

    public void SetChild(Machine newChild, bool userPlaced)
    {
        machine = newChild;

        if (newChild != null)
        {
            machine.gameObject.transform.position = new Vector3(transform.position.x, (0 + newChild.YOffset), transform.position.z);
            machine.SetDir(Direction.down);
            machine.Parent = this;
        }

        // Event
        if (userPlaced)
            EventManager.Instance.AddEvent(EventType.Machine_Placed);
    }

    #region PcControls

    // Mouse controls, specific to PC
    void OnMouseOver()
    {
#if UNITY_EDITOR

        if (Input.GetMouseButtonUp(0) && GameManager.Instance.ValidPress())
        {
            OnTouch(Input.mousePosition.x, Input.mousePosition.y);
        }

        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }
#endif
    }

    private void RightClick()
    {
        ToggleActive();
    }

    #endregion

    void OnTouch(float touchX, float touchY)
    {
        // Return unless we're active
        if (!active) { return; }

        // Return if the production line is running
        if (LevelController.Instance.Running) { return; }

        // Return if we tapped over UI
        if (Utility.IsOverUIObject(touchX, touchY)) { return; }

        // Check that the user is in an appropriate build mode
        // (What do they have selected?)
        switch (LevelController.Instance.BuildStatus)
        {
            /* Debug options */
            case BuildMode.input:
            case BuildMode.output:

            /* Regular options */
            case BuildMode.brewer:
            case BuildMode.conveyer:
            case BuildMode.grinder:
            case BuildMode.oven:
            case BuildMode.slow_conveyer:
            case BuildMode.rotate_conveyer:
                LevelController.Instance.SpawnOn(x, y);
                break;
        }
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

    public override void Brighten()
    {
        GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);

        if (active)
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        else
            gameObject.GetComponent<Renderer>().material.color = Color.black;
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