using UnityEngine;

[System.Serializable]
public class DungeonRoom
{
    public string roomName;
    public GameObject[] spawnPoints;
    public GameObject doorToNextRoom;
    public GameObject progressButton;
    public GameObject blackscreen;
    public bool black = true;
}
