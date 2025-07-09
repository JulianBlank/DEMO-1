using UnityEngine;
using System.Collections.Generic;

public class DungeonRoomManager : MonoBehaviour
{
    [Header("Gegner")]
    [Tooltip("Liste der möglichen Gegner-Prefabs, die gespawnt werden können.")]
    public GameObject[] enemyPrefabs;

    [Header("Räume in Reihenfolge")]
    [Tooltip("Reihenfolge der Dungeonräume.")]
    public List<DungeonRoom> dungeonRooms;

    // Index des aktuell aktiven Raums in der dungeonRooms-Liste
    private int currentRoomIndex = 0;

    // Anzahl der aktuell lebenden Gegner im Raum
    private int aliveEnemies = 0;

    // Unity-Methode, die beim Start des Spiels oder der Szene aufgerufen wird.
    // Initialisiert den ersten Dungeonraum.
    private void Start()
    {
        StartRoom(currentRoomIndex);
    }

    // Startet den Raum an der angegebenen Position in dungeonRooms.
    // Deaktiviert den Fortschrittsbutton und spawnt Gegner an definierten Spawnpunkten.
    private void StartRoom(int index)
    {
        if (index >= dungeonRooms.Count)
        {
            Debug.Log("Kein weiterer Raum vorhanden.");
            return;
        }

        DungeonRoom room = dungeonRooms[index];

        // Fortschrittsbutton deaktivieren, um voreilige Fortschritte zu verhindern
        if (room.progressButton != null)
            room.progressButton.SetActive(false);

        // Gegner an den festgelegten Spawnpunkten erzeugen
        SpawnEnemies(room.spawnPoints);
    }

    // Spawnt Gegner an den angegebenen Spawnpunkten.
    // Registriert den OnDeath-Callback für jeden Gegner, um den Fortschritt zu überwachen.
    private void SpawnEnemies(GameObject[] spawnPointObjects)
    {
        aliveEnemies = spawnPointObjects.Length;

        foreach (GameObject obj in spawnPointObjects)
        {
            Transform spawn = obj.transform;

            // Zufälliges Gegner-Prefab auswählen und instanziieren
            GameObject enemy = Instantiate(
                enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                spawn.position,
                Quaternion.identity
            );

            // Enemy-Komponente abrufen und OnDeath-Event abonnieren
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
                e.OnDeath += HandleEnemyDeath;
            else
                Debug.LogWarning("Enemy-Komponente fehlt auf instanziertem Gegner.");
        }
    }

    // Callback-Methode, die aufgerufen wird, wenn ein Gegner stirbt.
    // Reduziert die Anzahl lebender Gegner und aktiviert den Fortschrittsbutton,
    // sobald alle Gegner besiegt wurden.
    private void HandleEnemyDeath()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            DungeonRoom currentRoom = dungeonRooms[currentRoomIndex];

            // Fortschrittsbutton aktivieren, damit der Spieler zum nächsten Raum wechseln kann
            if (currentRoom.progressButton != null)
                currentRoom.progressButton.SetActive(true);
        }
    }

    // Wird vom Fortschrittsbutton des aktuellen Raums aufgerufen.
    // Zerstört die Tür zum nächsten Raum, blendet das Schwarzbild aus,
    // deaktiviert den Button und startet den nächsten Raum.
    public void OnRoomProgressButtonClicked()
    {
        DungeonRoom currentRoom = dungeonRooms[currentRoomIndex];

        // Tür zum nächsten Raum zerstören, damit der Spieler durchgehen kann
        if (currentRoom.doorToNextRoom != null)
            Destroy(currentRoom.doorToNextRoom);

        // Schwarzbild ausblenden, falls aktiv
        if (currentRoom.blackscreen != null && currentRoom.black == true)
        {
            SpriteFader fader = currentRoom.blackscreen.GetComponent<SpriteFader>();
            if (fader != null)
                fader.FadeOut(1f); // Dauer der Ausblendung in Sekunden
            currentRoom.black = false;
        }

        // Fortschrittsbutton deaktivieren, damit er nicht erneut gedrückt wird
        if (currentRoom.progressButton != null)
            currentRoom.progressButton.SetActive(false);

        // Nächsten Raum starten
        currentRoomIndex++;
        StartRoom(currentRoomIndex);
    }
}
