using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 5f;

    /// <summary>
    /// Wird vom Gegner aufgerufen, um das Projektil zu erzeugen und auf ein Ziel zu schicken.
    /// </summary>
    public static void CreateProjectile(GameObject projectilePrefab, Vector3 spawnPosition, Vector3 targetPosition)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile Prefab ist nicht gesetzt!");
            return;
        }

        GameObject projectileInstance = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetTargetPosition(targetPosition);
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
