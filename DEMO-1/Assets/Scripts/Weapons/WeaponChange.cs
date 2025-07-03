using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class AttachOnProximity : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 2f;
    public Vector3 offset = new Vector3(1f, 0f, 0f);
    public Sprite sword_sprite;
    private bool isAttached = false;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    private bool canpickup;
    private float resetpickup = 3f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canpickup = true;
        if (spriteRenderer == null)
        {
            Debug.LogWarning("No SpriteRenderer found on this GameObject.");
            return;
        }

        originalSprite = spriteRenderer.sprite; // Save original if you want to restore it later

        if (sword_sprite != null)
        {
            spriteRenderer.sprite = sword_sprite;
            Debug.Log("Sprite set at start.");
        }
        else
        {
            Debug.LogWarning("No sprite assigned to 'sword_sprite'.");
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
        if (canpickup == true)
        {
            StartCoroutine(ResetPickupcooldown());
            isAttached = true;
            canpickup = false;
            transform.SetParent(player);
            transform.localPosition = offset;

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = null;
            }

            Debug.Log("Object attached to player.");
        }
    }
    public bool GetStatus()
    {
        return isAttached;
    }
    private IEnumerator ResetPickupcooldown()
    {
        yield return new WaitForSeconds(resetpickup);
        canpickup = true;
        Debug.Log("Pick up ready again");
    }
    void Detach()
    {
        isAttached = false;
        transform.SetParent(null);

        if (spriteRenderer != null && sword_sprite != null)
        {
            spriteRenderer.sprite = sword_sprite;
        }

        Debug.Log("Object detached from player.");
    }
}
