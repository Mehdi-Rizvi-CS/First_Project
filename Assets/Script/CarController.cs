using UnityEngine;

// Attach this script to your car GameObject.
// Requires a Rigidbody component on the same GameObject.
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 12f;       // normal forward/backward speed
    public float turnSpeed = 100f;      // degrees per second

    [Header("Boost")]
    public float boostMultiplier = 2f;  // how much faster during boost
    public float boostDuration = 3f;    // boost lasts 3 seconds
    public float boostCooldown = 5f;    // wait time before boosting again

    private Rigidbody rb;
    private bool isBoosting = false;
    private bool canBoost = true;
    private float boostTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleBoostInput();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float vertical = Input.GetAxis("Vertical");   // W/S or Up/Down
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right

        float currentSpeed = moveSpeed * (isBoosting ? boostMultiplier : 1f);

        // Move forward/backward
        Vector3 forwardMove = transform.forward * vertical * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);

        // Turn left/right (only while moving, like a real car)
        float turn = horizontal * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    void HandleBoostInput()
    {
        // Start boost
        if (Input.GetKeyDown(KeyCode.LeftShift) && canBoost && !isBoosting)
        {
            isBoosting = true;
            boostTimer = boostDuration;
            canBoost = false;
        }

        // Count down boost duration
        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                isBoosting = false;
                cooldownTimer = boostCooldown;
            }
        }
        // Count down cooldown before boost is available again
        else if (!canBoost)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canBoost = true;
            }
        }
    }
}
