using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Gravity")]
public float gravityMultiplier = 1f;
public float fallMultiplier = 2f;

    [Header("Jump")]
public float jumpForce = 200f;

private bool isGrounded;

    [Header("Movement")]
    public float moveSpeed = 16f;

    private Rigidbody rb;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
{
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
}
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(horizontal, 0f, vertical).normalized;
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
    private void OnCollisionStay(Collision collision)
{
    isGrounded = true;
}

private void OnCollisionExit(Collision collision)
{
    isGrounded = false;
}
}