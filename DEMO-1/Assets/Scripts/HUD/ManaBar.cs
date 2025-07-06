using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Image manabar; // UI Image that fills

    void Start()
    {
        if (manabar == null)
        {
            Debug.LogError("Mana Bar Image not assigned!");
            return;
        }

        StartCoroutine(UpdateManaBarCoroutine());
    }

    private IEnumerator UpdateManaBarCoroutine()
    {
        while (true)
        {
            float manaPercent = 0.3f;
            manabar.fillAmount = manaPercent;
            yield return new WaitForSeconds(1f);
        }
    }
}
