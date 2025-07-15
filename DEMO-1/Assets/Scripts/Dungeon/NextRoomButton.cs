using UnityEngine;

public class RoomProgressButton : MonoBehaviour
{
    // Referenz auf den DungeonRoomManager, der den Raumfortschritt steuert
    public DungeonRoomManager roomManager;

    // Wird aufgerufen, wenn der Fortschrittsbutton gedrückt wird
    public void OnButtonClick()
    {
        // Prüfen, ob der DungeonRoomManager zugewiesen wurde
        if (roomManager != null)
        {
            // Raumfortschritt im Manager auslösen
            roomManager.OnRoomProgressButtonClicked();
        }
        else
        {
            // Warnung ausgeben, falls kein Manager gesetzt ist
            Debug.LogWarning("RoomManager nicht zugewiesen im Button!");
        }
    }
}
