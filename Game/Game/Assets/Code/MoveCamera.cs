using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [Header("Camera movement speed")]
    [SerializeField]
    private float scaleAmount;


    Vector2 touchPoint;
    Vector2 currentTouchPoint;
    Vector3 cameraStartPoint;

    void Update()
    {
        TouchInput();
        // MouseInput();
    }

    private void TouchInput()
    {
        // On the frame that the user touched...
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Get the point on the screen we touched
            touchPoint = Input.GetTouch(0).position;

            // Get the camera position when we touched the screen
            cameraStartPoint = transform.position;
        }

        // If the user is dragging...
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Where are we currently touching
            currentTouchPoint = Input.GetTouch(0).position;

            // What is the difference
            Vector2 difference = currentTouchPoint - touchPoint;
            float diffLength = difference.magnitude;

            // If we don't need to rotate the vector...
            if (transform.eulerAngles.y == 0)
            {
                // Scale it down
                difference *= scaleAmount;

                // Fix inversion
                difference.x = -difference.x;
                difference.y = -difference.y;

                // Move
                transform.position = new Vector3(cameraStartPoint.x + difference.x, cameraStartPoint.y, cameraStartPoint.z + difference.y);
            }
            // Else
            else
            {
                // Rotate the vector so it's pointing in the direction our camera is facing
                Vector2 normalizedDifference = RotateVectorToCameraAngle(difference, transform.eulerAngles.y);

                // This results in a normalized vector, therefore we now scale it
                // back up to where it was
                normalizedDifference *= diffLength;

                // Now we can scale it down as appropriate
                normalizedDifference.x *= scaleAmount;
                normalizedDifference.y *= scaleAmount;

                // Fix inversion
                normalizedDifference.x = -normalizedDifference.x;
                normalizedDifference.y = -normalizedDifference.y;

                transform.position = new Vector3(cameraStartPoint.x + normalizedDifference.x, cameraStartPoint.y, cameraStartPoint.z + normalizedDifference.y);
            }
        }
    }

    private void MouseInput()
    {
        // On the frame that we left click...
        if (Input.GetMouseButtonDown(0))
        {
            // Get the point on the screen we touched
            touchPoint = Input.mousePosition;

            // Get the camera position when we touched the screen
            cameraStartPoint = transform.position;
        }

        // If we're holding the mouse...
        if (Input.GetMouseButton(0))
        {
            // Where are we currently touching
            currentTouchPoint = Input.mousePosition;

            // What is the difference
            Vector2 difference = currentTouchPoint - touchPoint;
            float diffLength = difference.magnitude;

            // If we don't need to rotate the vector...
            if (transform.eulerAngles.y == 0)
            {
                // Scale it down
                difference *= scaleAmount;

                // Fix inversion
                difference.x = -difference.x;
                difference.y = -difference.y;

                // Move
                transform.position = new Vector3(cameraStartPoint.x + difference.x, cameraStartPoint.y, cameraStartPoint.z + difference.y);
            }
            // Else
            else
            {
                // Rotate the vector so it's pointing in the direction our camera is facing
                Vector2 normalizedDifference = RotateVectorToCameraAngle(difference, transform.eulerAngles.y);

                // This results in a normalized vector, therefore we now scale it
                // back up to where it was
                normalizedDifference *= diffLength;

                // Now we can scale it down as appropriate
                normalizedDifference.x *= scaleAmount;
                normalizedDifference.y *= scaleAmount;

                // Fix inversion
                normalizedDifference.x = -normalizedDifference.x;
                normalizedDifference.y = -normalizedDifference.y;

                transform.position = new Vector3(cameraStartPoint.x + normalizedDifference.x, cameraStartPoint.y, cameraStartPoint.z + normalizedDifference.y);
            }
        }
    }

    private Vector2 RotateVectorToCameraAngle(Vector2 vec, float deg)
    {
        if (deg == 0) { return vec; }

        // Convert to angle
        float vectorAngle = Mathf.Atan2(vec.x, vec.y);

        // Plus
        vectorAngle += deg;

        // Convert to vector
        Vector2 newVec = new Vector2(Mathf.Cos(vectorAngle), Mathf.Sin(vectorAngle));
        return newVec;
    }
}