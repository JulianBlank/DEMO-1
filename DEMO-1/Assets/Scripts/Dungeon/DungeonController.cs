using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Verwaltet Dungeonräume, spawnt Gegner und steuert den Fortschritt von Raum zu Raum.
/// </summary>
public class DungeonRoomManager : MonoBehaviour
{
    [Header("Gegner")]
    [Tooltip("Liste der möglichen Gegner-Prefabs, die gespawnt werden können.")]
    public GameObject[] enemyPrefabs;

    [Header("Räume in Reihenfolge")]
    [Tooltip("Reihenfolge der Dungeonräume.")]
    public List<DungeonRoom> dungeonRooms;

    private int currentRoomIndex = 0;
    private int aliveEnemies = 0;

    /// <summary>
    /// Initialisiert den ersten Raum beim Start.
    /// </summary>
    private void Start()
    {
        StartRoom(currentRoomIndex);
    }

    /// <summary>
    /// Startet den angegebenen Raum, deaktiviert den Fortschrittsbutton und spawnt Gegner.
    /// </summary>
    /// <param name="index">Index des Raums in der dungeonRooms-Liste.</param>
    private void StartRoom(int index)
    {
        if (index >= dungeonRooms.Count)
        {
            Debug.Log("Kein weiterer Raum vorhanden.");
            return;
        }

        DungeonRoom room = dungeonRooms[index];

        // Fortschrittsbutton deaktivieren
        if (room.progressButton != null)
            room.progressButton.SetActive(false);

        // Gegner spawnen
        SpawnEnemies(room.spawnPoints);
    }

    /// <summary>
    /// Spawnt Gegner an den angegebenen Spawnpunkten und registriert OnDeath-Callback.
    /// </summary>
    /// <param name="spawnPointObjects">Array von GameObjects, deren Position als Spawnpunkt dient.</param>
    private void SpawnEnemies(GameObject[] spawnPointObjects)
    {
        aliveEnemies = spawnPointObjects.Length;

        foreach (GameObject obj in spawnPointObjects)
        {
            Transform spawn = obj.transform;

            GameObject enemy = Instantiate(
                enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                spawn.position,
                Quaternion.identity
            );

            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
                e.OnDeath += HandleEnemyDeath;
            else
                Debug.LogWarning("Enemy-Komponente fehlt auf instanziertem Gegner.");
        }
    }

    /// <summary>
    /// Wird aufgerufen, wenn ein Gegner stirbt. Aktiviert bei 0 verbleibenden Gegnern den Fortschrittsbutton.
    /// </summary>
    private void HandleEnemyDeath()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            DungeonRoom currentRoom = dungeonRooms[currentRoomIndex];

            if (currentRoom.progressButton != null)
                currentRoom.progressButton.SetActive(true);
        }
    }

    /// <summary>
    /// Wird vom Fortschrittsbutton aufgerufen, zerstört die Tür, blendet Schwarzbild aus und startet nächsten Raum.
    /// </summary>
    public void OnRoomProgressButtonClicked()
    {
        DungeonRoom currentRoom = dungeonRooms[currentRoomIndex];

        // Tür zur nächsten Kammer zerstören
        if (currentRoom.doorToNextRoom != null)
            Destroy(currentRoom.doorToNextRoom);

        // Schwarzbild ausblenden
        if (currentRoom.blackscreen != null && currentRoom.black == true)
        {
            SpriteFader fader = currentRoom.blackscreen.GetComponent<SpriteFader>();
            if (fader != null)
                fader.FadeOut(1f);
            currentRoom.black = false;
        }

        // Button deaktivieren, damit er nur einmal genutzt werden kann
        if (currentRoom.progressButton != null)
            currentRoom.progressButton.SetActive(false);

        currentRoomIndex++;
        StartRoom(currentRoomIndex);
    }
}
