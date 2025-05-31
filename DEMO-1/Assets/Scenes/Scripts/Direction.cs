using UnityEngine;

public class RigidbodyMovementDirection : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject Player;

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = Player.GetComponent<Rigidbody2D>();

        // It's good practice to check if the Rigidbody exists
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject! Please ensure it's attached.");
            enabled = false; // Disable this script if Rigidbody is missing
        }
    }

    void Update()
    {
        // Ensure rb is not null before trying to access its properties
        if (rb != null)
        {
            // Get the current velocity of the Rigidbody
            Vector3 currentVelocity = rb.linearVelocity;

            // The direction of movement is the normalized velocity vector.
            // Normalizing a vector scales it to a length of 1, preserving its direction.
            Vector3 movementDirection = currentVelocity.normalized;

            // The speed of movement is the magnitude (length) of the velocity vector.
            float speed = currentVelocity.magnitude;

            // Output the direction and speed to the Unity console
            // You can use these values for your game logic (e.g., animations, AI, effects)
            Debug.Log("Current Movement Direction: " + movementDirection);
            Debug.Log("Current Speed: " + speed);

            // Example of how you might use the direction:
            // if (movementDirection.magnitude > 0) // Check if there's actual movement
            // {
            //     // E.g., orient the object to face its movement direction
            //     // transform.forward = movementDirection;
            // }
        }
    }
}