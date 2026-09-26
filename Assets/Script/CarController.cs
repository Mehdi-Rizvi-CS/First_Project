using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Movement")]
    public float accelerationForce = 25f;
    public float reverseForce = 15f;
    public float turnTorque = 8f;
    public float maxSpeed = 20f;

    [Header("Handling")]
    public float sidewaysFriction = 5f;
    public float angularDrag = 3f;

    [Header("Boost")]
    public float boostMultiplier = 2f;
    public float boostDuration = 3f;
    public float boostCooldown = 5f;

    private Rigidbody rb;

    private bool isBoosting = false;
    private bool canBoost = true;

    private float boostTimer = 0f;
    private float cooldownTimer = 0f;

    private float verticalInput;
    private float horizontalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.angularDamping = angularDrag;
    }

    void Update()
    {
        HandleInput();
        HandleBoostInput();
        HandleBoostTimers();
    }

    void FixedUpdate()
    {
        HandleMovement();
        LimitSpeed();
        ApplySidewaysFriction();
    }

    void HandleInput()
    {
        verticalInput = 0f;
        horizontalInput = 0f;

        if (Keyboard.current == null)
            return;

        // Forward
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
        {
            verticalInput = 1f;
        }

        // Reverse
        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
        {
            verticalInput = -1f;
        }

        // Left
        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            horizontalInput = -1f;
        }

        // Right
        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            horizontalInput = 1f;
        }
    }

    void HandleMovement()
    {
        float boost = isBoosting ? boostMultiplier : 1f;

        // FORWARD
        if (verticalInput > 0f)
        {
            rb.AddForce(
                transform.forward *
                accelerationForce *
                boost,
                ForceMode.Acceleration
            );
        }

        // REVERSE
        if (verticalInput < 0f)
        {
            rb.AddForce(
                -transform.forward *
                reverseForce,
                ForceMode.Acceleration
            );
        }

        // STEERING
        float forwardVelocity =
            Vector3.Dot(rb.linearVelocity, transform.forward);

        if (Mathf.Abs(forwardVelocity) > 0.5f)
        {
            float direction = forwardVelocity >= 0f ? 1f : -1f;

            rb.AddTorque(
                Vector3.up *
                horizontalInput *
                turnTorque *
                direction,
                ForceMode.Acceleration
            );
        }
    }

    void LimitSpeed()
    {
        Vector3 horizontalVelocity =
            new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

        float currentMaxSpeed =
            maxSpeed * (isBoosting ? boostMultiplier : 1f);

        if (horizontalVelocity.magnitude > currentMaxSpeed)
        {
            Vector3 limitedVelocity =
                horizontalVelocity.normalized * currentMaxSpeed;

            rb.linearVelocity =
                new Vector3(
                    limitedVelocity.x,
                    rb.linearVelocity.y,
                    limitedVelocity.z
                );
        }
    }

    void ApplySidewaysFriction()
    {
        Vector3 localVelocity =
            transform.InverseTransformDirection(rb.linearVelocity);

        localVelocity.x *=
            Mathf.Clamp01(1f - sidewaysFriction * Time.fixedDeltaTime);

        Vector3 newVelocity =
            transform.TransformDirection(localVelocity);

        rb.linearVelocity =
            new Vector3(
                newVelocity.x,
                rb.linearVelocity.y,
                newVelocity.z
            );
    }

    void HandleBoostInput()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame &&
            canBoost &&
            !isBoosting)
        {
            isBoosting = true;
            boostTimer = boostDuration;
            canBoost = false;
        }
    }

    void HandleBoostTimers()
    {
        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;

            if (boostTimer <= 0f)
            {
                isBoosting = false;
                cooldownTimer = boostCooldown;
            }
        }
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