using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    string displayName;
    public string DisplayName { get { return displayName; } }

    public int X
    {
        get { return x; }
        set { x = value; }
    }
    private int x;

    public int Y
    {
        get { return y; }
        set { y = value; }
    }
    private int y;

    void Start() { }

    void Update() { }

    public virtual void Tick()
    {
        return;
    }

    public virtual void Flush()
    {
        return;
    }

    public virtual void Execute()
    {
        return;
    }

    public virtual void OnClick()
    {
        LevelController lc = GameObject.Find("LevelController").GetComponent<LevelController>();
        lc.SpawnOn(x, y);
    }

    private void OnMouseDown()
    {
        OnClick();
    }

}
