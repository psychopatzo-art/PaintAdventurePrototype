using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0f, 7f, -10f);

    [Header("Follow Settings")]
    public float followSmoothness = 8f;

    [Header("Look Settings")]
    public float lookHeight = 1.5f;

    [Header("Collision Settings")]
    public float collisionRadius = 0.35f;
    public float collisionOffset = 0.3f;
    public LayerMask collisionLayers;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 lookTarget = target.position + Vector3.up * lookHeight;

        Vector3 desiredPosition = target.position + offset;

        Vector3 direction = desiredPosition - lookTarget;
        float desiredDistance = direction.magnitude;
        direction.Normalize();

        Vector3 finalPosition = desiredPosition;

        if (Physics.SphereCast(
            lookTarget,
            collisionRadius,
            direction,
            out RaycastHit hit,
            desiredDistance,
            collisionLayers
        ))
        {
            finalPosition = hit.point - direction * collisionOffset;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            finalPosition,
            followSmoothness * Time.deltaTime
        );

        transform.LookAt(lookTarget);
    }
}