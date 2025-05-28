using UnityEngine;

public class DragObject2D : MonoBehaviour
{
    private Vector3 offset; // Der Offset zwischen der Mausposition und der Objektposition

    void OnMouseDown()
    {
        // Wird aufgerufen, wenn die Maustaste über diesem Collider gedrückt wird.
        // Berechnet den Offset, damit das Objekt nicht "springt", wenn man es anklickt.
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
    }

    void OnMouseDrag()
    {
        // Wird aufgerufen, während die Maustaste gedrückt gehalten wird und über diesem Collider ist.
        // Konvertiert die aktuelle Mausposition auf dem Bildschirm in Weltkoordinaten.
        Vector3 newMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

        // Setzt die Position des Objekts auf die neue Mausposition plus den ursprünglichen Offset.
        // Wir ignorieren die Z-Achse, da wir in einer 2D-Szene sind.
        transform.position = new Vector3(newMousePosition.x + offset.x, newMousePosition.y + offset.y, transform.position.z);
    }
}