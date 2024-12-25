using UnityEngine;

public class SnowfallFollowCamera : MonoBehaviour
{
    public Transform mainCamera; // Reference to the main camera transform

    private Vector3 initialOffset; // Offset to maintain the initial y position of the particle system

    void Start()
    {
        // Calculate the initial offset between the particle system and the camera
        initialOffset = transform.position - mainCamera.position;
    }

    void Update()
    {
        // Update the position of the particle system to match the camera's x position, but keep its initial y and z
        transform.position = new Vector3(mainCamera.position.x + initialOffset.x, transform.position.y, transform.position.z);
    }
}
