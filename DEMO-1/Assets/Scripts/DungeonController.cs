using UnityEngine;
using System.Collections.Generic;

public class DungeonRoomManager : MonoBehaviour
{
    [Header("Gegner")]
    public GameObject[] enemyPrefabs;

    [Header("Räume in Reihenfolge")]
    public List<DungeonRoom> dungeonRooms;


    private int currentRoomIndex = 0;
    private int aliveEnemies = 0;

    void Start()
    {
        StartRoom(currentRoomIndex);
    }

    void StartRoom(int index)
    {
        if (index >= dungeonRooms.Count)
        {
            Debug.Log("Kein weiterer Raum vorhanden.");
            return;
        }

        DungeonRoom room = dungeonRooms[index];

        // Button deaktivieren (erscheint erst wenn Gegner tot)
        if (room.progressButton != null)
            room.progressButton.SetActive(false);

        // Gegner spawnen
        SpawnEnemies(room.spawnPoints);
    }

    void SpawnEnemies(GameObject[] spawnPointObjects)
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
            e.OnDeath += HandleEnemyDeath;
        }
    }

    void HandleEnemyDeath()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            DungeonRoom currentRoom = dungeonRooms[currentRoomIndex];

            if (currentRoom.progressButton != null)
            {
                currentRoom.progressButton.SetActive(true);
            }
        }
    }

    public void OnRoomProgressButtonClicked()
    {
        DungeonRoom currentRoom = dungeonRooms[currentRoomIndex];

        // Tür zerstören
        if (currentRoom.doorToNextRoom != null)
        {
            Destroy(currentRoom.doorToNextRoom);
        }
        if (currentRoom.blackscreen != null && currentRoom.black == true)
        {
            currentRoom.blackscreen.GetComponent<SpriteFader>().FadeOut(1f);
            currentRoom.black = false;
        }
        // Button nur einmal benutzbar machen
        if (currentRoom.progressButton != null)
        {
            currentRoom.progressButton.SetActive(false);
        }

        currentRoomIndex++;
        StartRoom(currentRoomIndex);
    }
}
