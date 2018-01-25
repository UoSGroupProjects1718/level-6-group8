using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrthoCameraDrag : MonoBehaviour
{
    [Header("Camera movement speed")]
    [SerializeField]
    private float moveSpeed;

    [Header("Camera bounds")]
    [SerializeField]
    private float minX;
    [SerializeField]
    private float minZ;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float maxZ;

    private Vector2 downPos;
    private Vector2 dragPos;

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            TouchController();
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            MouseController();
        }
    }

    private void MouseController()
    {
        // On click
        if (Input.GetMouseButtonDown(0))
        {
            downPos = Input.mousePosition;
        }

        // Held down
        if (Input.GetMouseButton(0))
        {
            dragPos = Input.mousePosition;
            UpdatePosition();
        }
    }

    private void TouchController()
    {
        // On press
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            downPos = Input.GetTouch(0).position;
        }

        // Dragging
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            dragPos = Input.GetTouch(0).position;
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        // Raycast to ensure that we didnt click on UI...
        if (Utility.IsOverUIObject(dragPos.x, dragPos.y)) { return; }

        // Find the difference
        Vector2 difference = downPos - dragPos;

        // Normalize, scale it;
        difference = difference.normalized * moveSpeed;

        // Rotate
        Vector2 move = difference.Rotate(45);
        move = move.Rotate(90);

        // Update pos
        Vector3 cameraPos = transform.position;
        Vector3 newPos = new Vector3(cameraPos.x + move.x, cameraPos.y, cameraPos.z + move.y);

        // Abide by boundaries
        if (newPos.x < minX) newPos.x = minX;
        else if (newPos.x > maxX) newPos.x = maxX;
        if (newPos.z < minZ) newPos.z = minZ;
        else if (newPos.z > maxZ) newPos.z = maxZ;

        transform.position = newPos;
    }



    /// <summary>
    /// Sets the maximum and minimum bounds for the camera.
    /// </summary>
    /// <param name="_minX">Minimum x boundary</param>
    /// <param name="_minY">Minimum z boundary</param>
    /// <param name="_maxX">Maximum x boundary</param>
    /// <param name="_maxY">maximum z boundary</param>
    public void UpdateCameraBounds(float _minX, float _minZ, float _maxX, float _maxZ)
    {
        minX = _minX;
        minZ = _minZ;

        maxX = _maxX;
        maxZ = _maxZ;
    }
}