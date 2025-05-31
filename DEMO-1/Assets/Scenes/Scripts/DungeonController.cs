using UnityEngine;

public class DungeonRoomManager : MonoBehaviour
{
    [Header("Gegner")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPointsInitial;

    [Header("Progress")]
    public GameObject interactButton;      // Legacy Button (wird aktiviert)
    public GameObject doorToNextRoom;      // Tür, die entfernt wird
    public GameObject[] spawnPointsNext;   // Weitere Spawnpunkte (werden aktiviert)

    private int aliveEnemies;

    void Start()
    {
        SpawnEnemies(spawnPointsInitial);
        if (interactButton != null) interactButton.SetActive(false); // Start: Button versteckt
    }

    void SpawnEnemies(Transform[] points)
    {
        aliveEnemies = points.Length;

        foreach (Transform point in points)
        {
            GameObject enemy = Instantiate(
                enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                point.position,
                Quaternion.identity
            );

            Enemy e = enemy.GetComponent<Enemy>();
            e.OnDeath += HandleEnemyDeath;
        }
    }

    void HandleEnemyDeath()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            Debug.Log("Alle Gegner besiegt!");
            if (interactButton != null)
                interactButton.SetActive(true); // Button freischalten
        }
    }
    public void ProgressToNextRoom()
    {
        Debug.Log("Raumfortschritt wird durchgeführt");

        // Tür löschen
        if (doorToNextRoom != null)
        {
            Destroy(doorToNextRoom);
            Debug.Log("Tür zum nächsten Raum entfernt");
        }

        // Neue Spawnpunkte aktivieren und umwandeln
        Transform[] spawnPoints = new Transform[spawnPointsNext.Length];
        for (int i = 0; i < spawnPointsNext.Length; i++)
        {
            GameObject obj = spawnPointsNext[i];
            if (obj != null)
            {
                obj.SetActive(true);
                spawnPoints[i] = obj.transform;
            }
        }

        // Gegner spawnen
        SpawnEnemies(spawnPoints);
    }
    
}