using UnityEngine;

public class Mover : MonoBehaviour
{
    public float moveSpeed = 5f; // Geschwindigkeit, mit der sich das Objekt bewegt
    public float moveRange = 5f; // Die Gesamtstrecke, die das Objekt zurücklegt (von der Startposition aus in beide Richtungen)
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Speichert die Startposition des Objekts
    }

    void Update()
    {
        // Berechnet die neue X-Position mit einem Sinus-Welle.
        // Time.time gibt die aktuelle Zeit seit dem Start des Spiels zurück.
        // Der Sinus-Wert oszilliert zwischen -1 und 1.
        // Durch Multiplikation mit moveRange / 2 bewegen wir uns in beide Richtungen von der Mitte aus.
        float newX = startPosition.x + Mathf.Sin(Time.time * moveSpeed) * (moveRange / 2f);

        // Aktualisiert die Position des Objekts
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}