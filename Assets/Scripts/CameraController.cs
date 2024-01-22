using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float keyboardPanSpeed; // Speed of panning the camera.
    public float mousePanSpeed; // How sensitive pans with mouse.

    public float zoomSpeed; // Speed of zooming in and out

    public float minY; // Minimum height (closest zoom)
    public float maxY; // Maximum height (farthest zoom)

    private Vector3 lastMousePosition; // To keep track of the last mouse position for panning

    void Start()
    {
        // Set initial rotation of the camera
        //transform.rotation = Quaternion.Euler(0, 45, 0); // Set Y rotation to 45 degrees
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from WASD keys
        float x = Input.GetAxis("Horizontal") * keyboardPanSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * keyboardPanSpeed * Time.deltaTime;

        // Translate the camera along the X and Z axes
        transform.Translate(x, 0, z, Space.World);

        // Get input from the mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        float newY = transform.position.y - scroll;
        if (newY >= minY && newY <= maxY) // Only zoom if we're not exceeding min/max zoom.
        {
            // Calculate new position for zooming
            // Move the camera along its local Z-axis (forward direction)
            Vector3 zoomDirection = transform.forward * scroll;
            Vector3 newPosition = transform.position + zoomDirection;

            // Apply the new position to the camera
            transform.position = newPosition;
        }

        // Panning the camera with the middle mouse button
        if (Input.GetMouseButtonDown(2)) // Middle mouse button pressed
        {
            lastMousePosition = Input.mousePosition; // Record the position of the mouse
        }

        if (Input.GetMouseButton(2)) // Middle mouse button held down
        {
            // Calculate the difference in position since the last frame
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Scale the movement by the sensitivity factor
            delta.x *= mousePanSpeed * Time.deltaTime;
            delta.y *= mousePanSpeed * Time.deltaTime;

            // Translate the camera inversely by delta (panning)
            transform.Translate(-delta.x, 0, -delta.y, Space.World);

            // Update lastMousePosition for the next frame
            lastMousePosition = Input.mousePosition;
        }
    }
}