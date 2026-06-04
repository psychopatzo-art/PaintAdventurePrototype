using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 16f;
    public float acceleration = 20f;
    public float deceleration = 25f;
    private Vector3 currentVelocity;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Jump")]
    public float jumpForce = 18f;

    [Header("Double Jump")]
    public int maxJumps = 2;

    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.15f;

    [Header("Air Control")]
    public float airControlMultiplier = 0.6f;

    [Header("Gravity")]
    public float gravityMultiplier = 2f;
    public float fallMultiplier = 5f;

    [Header("Ground Check")]
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayers;

    private Rigidbody rb;
    private Vector3 moveInput;

    private int jumpCount;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGrounded();

        Vector2 input = Vector2.zero;

input.x = Input.GetAxisRaw("Horizontal");
input.y = Input.GetAxisRaw("Vertical");

if (Gamepad.current != null)
{
    Vector2 stickInput = Gamepad.current.leftStick.ReadValue();

    if (stickInput.magnitude > 0.1f)
    {
        input = stickInput;
    }
}

moveInput = new Vector3(input.x, 0f, input.y).normalized;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
{
    jumpPressed = true;
}

if (jumpPressed)
{
    jumpBufferCounter = jumpBufferTime;
}
else
{
    jumpBufferCounter -= Time.deltaTime;
}

if (jumpBufferCounter > 0f)
{
    if (coyoteTimeCounter > 0f)
    {
        DoJump();

        jumpCount = 1;
        coyoteTimeCounter = 0f;
        jumpBufferCounter = 0f;
    }
    else if (jumpCount < maxJumps)
    {
        DoJump();

        jumpCount++;
        jumpBufferCounter = 0f;
    }
}
    }

    private void FixedUpdate()
    {
Vector3 moveDirection = moveInput;

if (cameraTransform != null)
{
    Vector3 cameraForward = cameraTransform.forward;
    Vector3 cameraRight = cameraTransform.right;

    cameraForward.y = 0f;
    cameraRight.y = 0f;

    cameraForward.Normalize();
    cameraRight.Normalize();

    moveDirection = cameraForward * moveInput.z + cameraRight * moveInput.x;
    moveDirection.Normalize();
}

Vector3 targetVelocity = moveDirection * moveSpeed;

float currentAcceleration = acceleration;

if (!isGrounded)
{
    currentAcceleration *= airControlMultiplier;
}

float controlRate = moveInput.magnitude > 0f
    ? currentAcceleration
    : deceleration;
    
currentVelocity = Vector3.MoveTowards(
    currentVelocity,
    targetVelocity,
    controlRate * Time.fixedDeltaTime
);

Vector3 velocity = new Vector3(
    currentVelocity.x,
    rb.linearVelocity.y,
    currentVelocity.z
);

rb.linearVelocity = velocity;

        if (rb.linearVelocity.y < 0f)
        {
            rb.AddForce(Physics.gravity * fallMultiplier, ForceMode.Acceleration);
        }
        else if (rb.linearVelocity.y > 0f)
        {
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    private void DoJump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGrounded()
{
    bool wasGrounded = isGrounded;

    isGrounded = Physics.Raycast(
        transform.position,
        Vector3.down,
        groundCheckDistance,
        groundLayers
    );

    if (!wasGrounded && isGrounded)
    {
        jumpCount = 0;
    }

    if (wasGrounded && !isGrounded)
    {
        jumpCount = 1;
    }
}
}