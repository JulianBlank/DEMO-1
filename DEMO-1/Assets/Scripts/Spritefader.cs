using UnityEngine;
using System.Collections;

public class SpriteFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("SpriteFader requires a SpriteRenderer.");
    }

    public void FadeOut(float duration)
    {
        if (spriteRenderer != null)
            StartCoroutine(FadeAlpha(1f, 0f, duration));
    }

    public void FadeIn(float duration)
    {
        if (spriteRenderer != null)
            StartCoroutine(FadeAlpha(0f, 1f, duration));
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = spriteRenderer.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(from, to, elapsed / duration);
            spriteRenderer.color = new Color(c.r, c.g, c.b, newAlpha);
            yield return null;
        }

        spriteRenderer.color = new Color(c.r, c.g, c.b, to);
    }
}
