using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public Transform target; // Drag your Sphere here
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate() // Use LateUpdate so the camera follows after the ball moves
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yRotation += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseDelta.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to the camera object
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Keep the camera behind/above the ball
        transform.position = target.position - (transform.forward * 5f) + Vector3.up * 2f;
    }
}