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

    private float yaw;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = target.position + Vector3.up * targetHeight;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSmoothness * Time.deltaTime
        );

        if (Gamepad.current != null)
        {
            Vector2 lookInput = Gamepad.current.rightStick.ReadValue();

            yaw += lookInput.x * rotationSpeed * Time.deltaTime;
        }

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}