using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;         // The character to follow
    
    [Header("Camera Settings")]
    public float distance = 5.0f;    // Distance behind the character
    public float height = 2.0f;      // Height offset above the character
    public float rotationSpeed = 5f; // How quickly the camera rotates with the mouse
    public float yMinLimit = -40f;   // Minimum vertical angle
    public float yMaxLimit = 80f;    // Maximum vertical angle

    // Current rotation angles
    private float currentX = 0f;
    private float currentY = 0f;

    void Update()
    {
        // Get mouse movement
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        
        // Clamp vertical rotation
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
    }

    void LateUpdate()
    {
        if (!target) return;

        // Build a rotation from current X/Y
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // Calculate desired position
        Vector3 offset = new Vector3(0, height, -distance);
        Vector3 newPosition = target.position + rotation * offset;

        // Move this CameraRig to that position
        transform.position = newPosition;

        // Always look at the target
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}
