using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Speed of drag
    public float dragSpeed = 1;

    // Movement Vector
    private Vector3 move;
    // Origin of each drag
    private Vector3 dragOrigin;

    // May be neeed later for limit drag movement 
    //private Vector3 posOrigin;

    void Update()
    {
        // If left mouse button was clicked this frame
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        // If the left mouse button is not being held
        if (!Input.GetMouseButton(0)) return;

        // Set a new direction vector
        Vector3 direction = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);

        // Set Movement vector with speed
        move = new Vector3(direction.x * dragSpeed,0 , direction.y * dragSpeed);
        // Adjust for camera Angle
        /// TODO: make this use transform.rotation
        move = Quaternion.AngleAxis(-45, Vector3.up) * move;

        // Move the Camera
        transform.Translate(-move, Space.World);
    }
}