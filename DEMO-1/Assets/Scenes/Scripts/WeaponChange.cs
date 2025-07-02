using UnityEngine;

public class AttachOnProximity : MonoBehaviour
{
    public Transform player;              // Reference to the player
    public float triggerDistance = 2f;    // Distance to trigger attachment
    public Vector3 offset = new Vector3(1f, 0f, 0f); // Offset relative to player

    private bool isAttached = false;
    private Renderer objectRenderer;
    private Texture originalTexture;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null && objectRenderer.material.HasProperty("_MainTex"))
        {
            originalTexture = objectRenderer.material.mainTexture;
        }
        else
        {
            Debug.LogWarning("Object does not have a Renderer with a _MainTex property.");
        }
    }

    void Update()
    {
        if (!isAttached && Vector2.Distance(transform.position, player.position) <= triggerDistance)
        {
            Attach();
        }

        if (isAttached && Input.GetKey(KeyCode.Q))
        {
            Detach();
        }

        if (isAttached)
        {
            transform.position = player.position + offset;
        }
    }

    void Attach()
    {
        isAttached = true;
        if (objectRenderer != null)
        {
            objectRenderer.material.mainTexture = null;
        }
    }

    void Detach()
    {
        isAttached = false;
        if (objectRenderer != null && originalTexture != null)
        {
            objectRenderer.material.mainTexture = originalTexture;
        }
    }
}
