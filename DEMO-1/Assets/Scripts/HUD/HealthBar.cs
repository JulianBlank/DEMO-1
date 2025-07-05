using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFillImage; // UI Image that fills
    [SerializeField] private PlayerHealth ph;

    private RectTransform rectTransform;

    void Start()
    {
        // Now healthFillImage is definitely assigned
        rectTransform = healthFillImage.GetComponent<RectTransform>();
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (rectTransform != null && ph != null)
        {
            float healthPercent = (float)ph.getcurrentHealth() / ph.getmaxhealth();
            Vector3 newScale = rectTransform.localScale;
            newScale.y = healthPercent;
            rectTransform.localScale = newScale;
        }
    }
}
