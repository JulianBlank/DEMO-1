using UnityEngine;

[System.Serializable]
public class DungeonRoom
{
    public string roomName; // Optional: f�r �bersichtlichkeit
    public GameObject[] spawnPoints;
    public GameObject doorToNextRoom;
    public GameObject progressButton;
}
