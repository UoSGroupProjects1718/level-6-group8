using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCamera : MonoBehaviour
{
    [Header("Camera movement speed")]
    [SerializeField]
    private float scaleAmount;

    [Header("Camera zoom speed")]
    [SerializeField]
    private float perspectiveZoomSpeed;

    [Header("Camera bounds")]
    [SerializeField]
    private int minX;
    [SerializeField]
    private int maxX;
    [SerializeField]
    private int minZ;
    [SerializeField]
    private int maxZ;

    // Perspective zoom limits
    int zoomMin = 30;
    int zoomMax = 60;

    // Camera pinch to zoom speed on ortho
    float orthoZoomSpeed = .5f;

    // Camera movement
    Vector2 touchPoint;
    Vector2 currentTouchPoint;
    Vector3 cameraStartPoint;

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            TouchDrag();
            TouchPinchToZoom();
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            MouseInput();
        }
    }

    public void UpdateBounds(int _minX, int _maxX, int _minZ, int _maxZ)
    {
        minX = _minX;
        maxX = _maxX;
        minZ = _minZ;
        maxZ = _maxZ;
    }

    private void TouchDrag()
    {
        // On the frame that the user touched...
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Raycast to ensure that we didnt tap on UI...
            if (IsPointerOverUIObject(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y)) { return; }

            // Get the point on the screen we touched
            touchPoint = Input.GetTouch(0).position;

            // Get the camera position when we touched the screen
            cameraStartPoint = transform.position;
        }

        // If the user is dragging...
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Raycast to ensure that we didnt tap on UI...
            if (IsPointerOverUIObject(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y)) { return; }

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

                // Update pos
                UpdateCameraPosition(difference.x, difference.y);
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

                // Update pos
                UpdateCameraPosition(normalizedDifference.x, normalizedDifference.y);
            }
        }
    }

    private void TouchPinchToZoom()
    {
        // https://unity3d.com/learn/tutorials/topics/mobile-touch/pinch-zoom

        // Only pinch to zoom if there are two touches in the device
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
            float touchDeltaMag = (touchZero.position - touchOnePreviousPosition).magnitude;

            float deltaMagnitideDiff = prevTouchDeltaMag - touchDeltaMag;

            if (Camera.main.orthographic)
            {
                Camera.main.orthographicSize += deltaMagnitideDiff * orthoZoomSpeed;
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, .1f);
            }
            else
            {
                Camera.main.fieldOfView += deltaMagnitideDiff * perspectiveZoomSpeed;
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomMin, zoomMax);
            }
        }
    }

    private void MouseInput()
    {
        // On the frame that we left click...
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to ensure that we didnt click on UI...
            if (IsPointerOverUIObject(Input.mousePosition.x, Input.mousePosition.y)) { return; }

            // Get the point on the screen we touched
            touchPoint = Input.mousePosition;

            // Get the camera position when we touched the screen
            cameraStartPoint = transform.position;
        }

        // If we're holding the mouse...
        if (Input.GetMouseButton(0))
        {
            // Raycast to ensure that we didnt click on UI...
            if (IsPointerOverUIObject(Input.mousePosition.x, Input.mousePosition.y)) { return; }

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

                // Update camera pos
                UpdateCameraPosition(difference.x, difference.y);
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

                // Update camera pos
                UpdateCameraPosition(normalizedDifference.x, normalizedDifference.y);
            }
        }
    }

    private void UpdateCameraPosition(float diffX, float diffZ)
    {
        // Find our new X and Z positions
        float newX = cameraStartPoint.x + diffX;
        float newZ = cameraStartPoint.z + diffZ;

        // Keep them within our bounds
        if (newX < minX) newX = minX;
        else if (newX > maxX) newX = maxX;

        if (newZ < minZ) newZ = minZ;
        else if (newZ > maxZ) newZ = maxZ;

        // Set pos
        transform.position = new Vector3(newX, transform.position.y, newZ);
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

    /// <summary>
    /// Checks to see if the provided (x, y) coordinate is over a UI object
    /// </summary>
    /// <param name="x">Mouse/Touch x pos</param>
    /// <param name="y">Mouse/Touch y pos</param>
    /// <returns>True if over a UI object, otherwise false</returns>
    private bool IsPointerOverUIObject(float x, float y)
    {
        // https://answers.unity.com/questions/1073979/android-touches-pass-through-ui-elements.html

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(x, y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return (results.Count > 0);
    }
}