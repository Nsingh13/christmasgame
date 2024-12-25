using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // The character (or object) to follow
    public float smoothSpeed = 0.125f; // Smoothing speed for camera movement
    public Vector3 offset;         // Offset to maintain between the camera and the target

    private void FixedUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position
            Vector3 desiredPosition = target.position + offset;

            // Smoothly interpolate to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera position
            transform.position = smoothedPosition;
        }
    }
}
