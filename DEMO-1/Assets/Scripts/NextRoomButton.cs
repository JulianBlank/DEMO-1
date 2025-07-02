using UnityEngine;

public class RoomProgressButton : MonoBehaviour
{
    public DungeonRoomManager roomManager;

    public void OnButtonClick()
    {
        if (roomManager != null)
        {
            roomManager.OnRoomProgressButtonClicked();
        }
        else
        {
            Debug.LogWarning("RoomManager nicht zugewiesen im Button!");
        }
    }
}