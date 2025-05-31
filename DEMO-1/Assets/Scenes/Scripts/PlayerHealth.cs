using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        //Debug.Log("Spieler hat Schaden genommen! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            //Debug.Log("Spieler ist tot (Logik folgt später)");
            // Hier später Respawn/Restart/Deathscreen
        }
    }
}