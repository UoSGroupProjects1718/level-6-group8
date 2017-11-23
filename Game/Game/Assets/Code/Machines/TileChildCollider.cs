using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChildCollider : MonoBehaviour
{
    void OnMouseOver()
    {
        Transform parent = transform.parent;

        if (Input.GetMouseButtonDown(0))
        {
            parent.GetComponent<Tile>().LeftClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            parent.GetComponent<Tile>().RightClick();
        }
    }
}
