using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFillImage; // UI Image that fills
    [SerializeField] private PlayerHealth ph;

    void Start()
    {
        if (healthFillImage == null)
        {
            Debug.LogError("Health Fill Image not assigned!");
            return;
        }

        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthFillImage != null && ph != null)
        {
            float healthPercent = Mathf.Clamp01((float)ph.getcurrentHealth() / ph.getmaxhealth());
            healthFillImage.fillAmount = healthPercent;
        }
    }
}
