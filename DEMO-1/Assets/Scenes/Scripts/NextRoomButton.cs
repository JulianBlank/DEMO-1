using UnityEngine;

public class RoomProgressButton : MonoBehaviour
{
    public GameObject doorToDestroy;
    public GameObject[] spawnPointsToActivate;
    public DungeonRoomManager roomManager; // <--- NEU: Referenz auf den RoomManager

    public void OnButtonClick()
    {
        Debug.Log("Button gedrückt!");

        if (doorToDestroy != null)
        {
            Destroy(doorToDestroy);
            Debug.Log("Tür zerstört!");
        }

        foreach (GameObject obj in spawnPointsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        if (roomManager != null)
        {
            roomManager.ProgressToNextRoom(); // <--- Gegner an neuen Punkten spawnen
        }
        else
        {
            Debug.LogWarning("RoomManager fehlt im Button!");
        }
    }
}