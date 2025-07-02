using UnityEngine;

public class AttachOnProximity : MonoBehaviour
{
    public Transform player;                  // Reference to the player
    public float triggerDistance = 2f;        // Distance to trigger attachment
    public Vector3 offset = new Vector3(1f, 0f, 0f); // Offset relative to player
    public Texture sword_tex;                // Texture to apply at start

    private bool isAttached = false;
    private Renderer objectRenderer;
    private Texture originalTexture;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null && objectRenderer.material.HasProperty("_MainTex"))
        {
            originalTexture = objectRenderer.material.mainTexture;
            objectRenderer.material.mainTexture = sword_tex;
        }
        else
        {
            Debug.LogWarning("Object does not have a Renderer with a _MainTex property.");
        }
    }

    void Update()
    {
        if (!isAttached && Vector3.Distance(transform.position, player.position) <= triggerDistance)
        {
            Attach();
        }

        if (isAttached && Input.GetKeyDown(KeyCode.Q))
        {
            Detach();
        }
    }

    void Attach()
    {
        isAttached = true;

        // Parent to player for smooth movement and rotation tracking
        transform.SetParent(player);
        transform.localPosition = offset;

        // Optionally remove texture when attached
        if (objectRenderer != null)
        {
            objectRenderer.material.mainTexture = null;
        }

        Debug.Log("Object attached to player.");
    }

    void Detach()
    {
        isAttached = false;

        // Unparent and keep current world position
        transform.SetParent(null);

        // Restore the original texture
        if (objectRenderer != null && originalTexture != null)
        {
            objectRenderer.material.mainTexture = originalTexture;
        }

        Debug.Log("Object detached from player.");
    }
}
