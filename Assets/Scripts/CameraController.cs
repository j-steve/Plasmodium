using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed; // Speed of panning the camera.
    public float sensitivity; // How sensitive it with mouse.

    public float zoomSpeed; // Speed of zooming in and out

    public float minY; // Minimum height (closest zoom)
    public float maxY; // Maximum height (farthest zoom)


    void Start()
    {
        // Set initial rotation of the camera
        //transform.rotation = Quaternion.Euler(0, 45, 0); // Set Y rotation to 45 degrees
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from WASD keys
        float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

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
    }
}