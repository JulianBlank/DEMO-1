using UnityEngine;

public class AttachOnProximity : MonoBehaviour
{
    public Transform player;          // Reference to the player
    public float triggerDistance = 2f; // Distance to trigger attachment
    public Vector3 offset = new Vector3(1f, 0f, 0f); // Offset relative to player (e.g., in front)

    private bool isAttached = false;

    void Update()
    {
        if (!isAttached && Vector2.Distance(transform.position, player.position) <= triggerDistance)
        {
            isAttached = true;
            // Optionally disable physics or collisions here
        }
        if (istAttached == true && Input.GetKey(KeyCode.A) == true=
        {
            isAttached = false;
        }
        if (isAttached)
        {
            transform.position = player.position + offset;
        }
    }
}
