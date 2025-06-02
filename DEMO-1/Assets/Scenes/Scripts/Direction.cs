using UnityEngine;

public class MovementDirection : MonoBehaviour
{
    public Rigidbody2D rb;

    void Update()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.magnitude < 0.1f)
        {
            Debug.Log("Not moving");
            return;
        }

        string direction = "";

        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            direction = velocity.x > 0 ? "East" : "West";
        }
        else
        {
            direction = velocity.y > 0 ? "North" : "South";
        }

        Debug.Log("Moving " + direction);
    }
}
