using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    private Rigidbody rb;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(horizontal, 0f, vertical).normalized;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = moveInput * moveSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }
}