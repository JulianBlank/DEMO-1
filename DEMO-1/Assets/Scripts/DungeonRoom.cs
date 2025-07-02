using UnityEngine;

[System.Serializable]
public class DungeonRoom
{
    public string roomName; // Optional: für Übersichtlichkeit
    public GameObject[] spawnPoints;
    public GameObject doorToNextRoom;
    public GameObject progressButton;
}
