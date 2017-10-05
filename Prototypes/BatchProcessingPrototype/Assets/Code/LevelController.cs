using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject[] placeables;
    private Placeable child;

    void Start()
    { }

    void Update()
    {
        InputController();

        if (child != null)
        {
            PlaceableController();
        }
    }

    void InputController()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyChild();
        }
    }

    void PlaceableController()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        child.transform.position = new Vector2(mousePos.x, mousePos.y);
    }

    public void SpawnPlaceable(int index)
    {
        if (index < 0) { return; }
        else  if (index > placeables.Length) { return; }

        DestroyChild();

        child = Instantiate(placeables[index]).GetComponent<Placeable>();
    }


    private void DestroyChild()
    {
        if (child != null)
        {
            Destroy(child.gameObject);
        }
    }
}
