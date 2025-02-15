using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2.0f;   // Mouse look sensitivity
    public float moveSpeed = 5.0f;     // Movement speed
    public float verticalClamp = 80f;  // Limits vertical look angle

    private float rotationX = 0f;      // Track vertical rotation
    private bool isCameraActive = true; // Toggle for mouse movement

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        CheckForCursorUnlock();
    }

    void HandleMouseLook()
    {
        if (!isCameraActive) return; // Stop mouse look if disabled

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate camera horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically (with clamp)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalClamp, verticalClamp);
        transform.localRotation = Quaternion.Euler(rotationX, transform.localRotation.eulerAngles.y, 0);
    }

    void HandleMovement()
    {
        if (!isCameraActive) return; // Stop movement if disabled

        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection.y = 0; // Prevent unintended vertical movement

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void CheckForCursorUnlock()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // esc to unlock cursor
        {
            isCameraActive = false;
            Cursor.lockState = CursorLockMode.None; // Free cursor
            Cursor.visible = true; 
        }
        else if (Input.GetMouseButtonDown(0) && !isCameraActive) // click to renable cursor lock
        {
            isCameraActive = true;
            Cursor.lockState = CursorLockMode.Locked; // Re-lock cursor
            Cursor.visible = false;
        }
    }
}
