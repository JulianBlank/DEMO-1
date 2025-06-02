using UnityEngine;

public class SpriteRuntimeEditor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // This method can now be called from other scripts
    public void ChangeSprite(Sprite newSprite)
    {
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }
}
