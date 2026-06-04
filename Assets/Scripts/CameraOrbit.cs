using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow")]
    public float followSmoothness = 12f;
    public float targetHeight = 1.5f;

    [Header("Rotation")]
    public float rotationSpeed = 180f;
    public float pitchSpeed = 120f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    [Header("Camera Distance")]
    public float cameraDistance = 9f;
    public float cameraHeight = 1f;

    [Header("Collision")]
    public float collisionRadius = 0.35f;
    public float collisionOffset = 0.3f;
    public LayerMask collisionLayers;

    private float yaw;
    private float pitch = 25f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 pivotTargetPosition = target.position + Vector3.up * targetHeight;

        transform.position = Vector3.Lerp(
            transform.position,
            pivotTargetPosition,
            followSmoothness * Time.deltaTime
        );

        if (Gamepad.current != null)
        {
            Vector2 lookInput = Gamepad.current.rightStick.ReadValue();

            yaw += lookInput.x * rotationSpeed * Time.deltaTime;
            pitch -= lookInput.y * pitchSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 desiredCameraPosition =
            transform.position
            + rotation * new Vector3(0f, cameraHeight, -cameraDistance);

        Vector3 direction = desiredCameraPosition - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();

        Vector3 finalCameraPosition = desiredCameraPosition;

        if (Physics.SphereCast(
            transform.position,
            collisionRadius,
            direction,
            out RaycastHit hit,
            distance,
            collisionLayers
        ))
        {
            finalCameraPosition = hit.point - direction * collisionOffset;
        }

        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            mainCamera.transform.position = finalCameraPosition;
            mainCamera.transform.LookAt(transform.position);
        }

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}