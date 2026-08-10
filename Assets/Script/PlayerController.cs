using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private InputSystem_Actions inputActions;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isGrounded;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    [Header("Animations")]
    private Animator animator;

    private float xRotation = 0f;
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
        
        
        animator = GetComponent<Animator>(); 

        
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        Move();
        Look();
        ApplyGravity();
        UpdateAnimation(); 
    }

    private void Move()
    {
        Vector3 move =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void Look()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }



    private void UpdateAnimation()
    {



        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        animator.SetBool("isRunning", isMoving);


        // 2. Sideways movement logic from your screenshot
        // moveInput.x is negative when pressing 'A' (Left) and positive when pressing 'D' (Right)
        bool turningLeft = moveInput.x < -0.1f;
        bool turningRight = moveInput.x > 0.1f;

        isGrounded = controller.isGrounded;
        animator.SetBool("isJumping", !isGrounded);


        // Send the parameters to your Animator Controller


        animator.SetBool("TurnLeft", turningLeft);
        animator.SetBool("TurnRight", turningRight);




    }





}