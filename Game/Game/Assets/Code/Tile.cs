using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    string displayName;
    public string DisplayName { get { return displayName; } }

    [SerializeField]
    Machine machine;
    public Machine Machine { get { return machine; } }

    public int X
    {
        get { return x; }
        set { x = value; }
    }
    [SerializeField]
    private int x;

    public int Y
    {
        get { return y; }
        set { y = value; }
    }
    [SerializeField]
    private int y;

    void Start() { }

    void Update() { }

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

    public virtual void OnClick()
    {
        LevelController lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        lc.SpawnOn(x, y);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }

    private void OnMouseDown()
    {
        OnClick();
    }

}
