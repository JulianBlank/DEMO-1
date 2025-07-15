using UnityEngine;

[System.Serializable]
public class DungeonRoom
{
    // Name des Dungeonraums, zur besseren Identifikation im Editor oder Debugging
    public string roomName;

    // Array von GameObjects, die als Spawnpunkte für Gegner dienen
    public GameObject[] spawnPoints;

    // Referenz auf die Tür zum nächsten Raum, die zerstört wird, wenn man weitergehen kann
    public GameObject doorToNextRoom;

    // Fortschrittsbutton, der aktiviert wird, wenn alle Gegner besiegt sind, um zum nächsten Raum zu gelangen
    public GameObject progressButton;

    // Schwarzes Bild (UI-Element), das zum Überblenden vor oder nach dem Raum genutzt wird
    public GameObject blackscreen;

    // Status, ob das Schwarzbild aktuell aktiv ist (true = sichtbar)
    public bool black = true;
}
