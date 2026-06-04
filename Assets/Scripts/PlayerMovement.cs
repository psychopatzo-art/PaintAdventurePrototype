using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 16f;

    [Header("Jump")]
    public float jumpForce = 18f;

    [Header("Double Jump")]
    public int maxJumps = 2;

    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;

    [Header("Jump Buffer")]
public float jumpBufferTime = 0.15f;

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

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(horizontal, 0f, vertical).normalized;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
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
        Vector3 velocity = moveInput * moveSpeed;
        velocity.y = rb.linearVelocity.y;

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