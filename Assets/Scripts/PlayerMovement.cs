using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 16f;

    [Header("Jump")]
    public float jumpForce = 18f;

    [Header("Double Jump")]
    public int maxJumps = 2;

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

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            jumpCount++;
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
    }
}