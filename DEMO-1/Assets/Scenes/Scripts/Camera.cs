using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;           // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Smoothing factor
    public Vector3 offset;             // Optional offset from the player

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position + offset;
        targetPosition.z = transform.position.z; // Keep original Z position for 2D

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}
