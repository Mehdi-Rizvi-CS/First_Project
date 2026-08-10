using UnityEngine;
using UnityEngine.InputSystem;

public class SphereMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform cameraTransform; // Drag your Main Camera here in the Inspector
    
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
{
    rb = GetComponent<Rigidbody>();
    
    // This adds resistance to movement, making the ball stop faster
    rb.linearDamping = 1f; 
    
    // This stops the ball from spinning endlessly
    rb.angularDamping = 2f; 
}

    void Update()
    {
        // Ground check (simple)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.6f);

        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
{
    Vector2 input = Vector2.zero;
    if (Keyboard.current.wKey.isPressed) input.y += 1;
    if (Keyboard.current.sKey.isPressed) input.y -= 1;
    if (Keyboard.current.dKey.isPressed) input.x += 1;
    if (Keyboard.current.aKey.isPressed) input.x -= 1;

    Vector3 forward = cameraTransform.forward;
    Vector3 right = cameraTransform.right;
    forward.y = 0f; right.y = 0f;
    forward.Normalize(); right.Normalize();

    Vector3 movement = (forward * input.y + right * input.x).normalized;

    // Use AddForce instead of MovePosition for better terrain interaction
    rb.AddForce(movement * moveSpeed * 10f, ForceMode.Acceleration);
}

}
































// using UnityEngine;
// using UnityEngine.InputSystem;

// public class SphereMovement : MonoBehaviour
// {
//     public float moveSpeed = 5f;
//     public float jumpForce = 5f;
//     private Rigidbody rb;
//     private bool isGrounded;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     void Update()
//     {
//         // Simple check to see if we are on the ground (y-position close to 0)
//         // For more complex games, consider using Physics.CheckSphere or Collision detection
//         isGrounded = transform.position.y <= 0.55f;

//         // Jump input
//         if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
//         {
//             rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//         }
//     }

//     void FixedUpdate()
//     {
//         // Movement input
//         Vector2 input = Vector2.zero;
//         if (Keyboard.current.wKey.isPressed) input.y += 1;
//         if (Keyboard.current.sKey.isPressed) input.y -= 1;
//         if (Keyboard.current.dKey.isPressed) input.x += 1;
//         if (Keyboard.current.aKey.isPressed) input.x -= 1;

//         Vector3 movement = new Vector3(input.x, 0.0f, input.y);
//         rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
//     }
// }


