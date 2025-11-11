using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // Player movement that works with ThirdPersonCamera
    // Moves relative to camera direction in all modes
    // Includes collision, jump (if needed), and normalized diagonal movement (prevents faster diagonal movement)

    [Header("Movement Settings")]
    [Tooltip("Ground movement speed")]
    public float moveSpeed = 5f;

    [Tooltip("Jump force (vertical movement)")]
    public float jumpForce = 5f;

    [Tooltip("Gravity multiplier when falling")]
    public float gravityMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;

    [Tooltip("Radius of ground detection sphere")]
    public float groundCheckRadius = 0.2f;

    public LayerMask groundLayer;

    [Header("Camera Reference")]
    [Tooltip("Reference Farhan's Camera Script")]
    public Transform cameraTransform;


    // Internal Components

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 moveDirection;


    void Start()
    {
        // Get or add Rigidbody for physics-based collision
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

 
        rb.freezeRotation = true; // Prevent physics from rotating player
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Auto-find camera if not assigned
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Create ground check point if not assigned
        if (groundCheck == null)
        {
            GameObject check = new GameObject("GroundCheck");
            check.transform.SetParent(transform);
            check.transform.localPosition = new Vector3(0, -1f, 0);
            groundCheck = check.transform;
        }
    }

    //  INPUT & MOVEMENT CALCULATION

    void Update()
    {
        // Check if player is on ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Get input
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrows
        float vertical = Input.GetAxisRaw("Vertical");     // W/S or Up/Down arrows

        // Calculate movement direction relative to camera
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical);

        // Normalize diagonal movement to prevent faster speed
        if (inputDirection.magnitude > 1f)
        {
            inputDirection.Normalize();
        }

        // Get camera's forward and right directions (flatten to horizontal plane)
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Remove vertical component to keep movement horizontal
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate final movement direction relative to camera
        moveDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x);

        // Rotate player to face movement direction (optional - comment out for strafing)
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Handle jump (Space bar for up/down movement)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

 
    // Apply movement using Rigidbody for proper collision

    void FixedUpdate()
    {
        // Apply horizontal movement
        Vector3 targetVelocity = moveDirection * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y; // Preserve vertical velocity

        rb.linearVelocity = targetVelocity;

        // Apply enhanced gravity when falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

}