using UnityEngine;

public class DragObject2D : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;

    void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePosition.x, mousePosition.y, 0f);
        dragging = true;
    }

    void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f) + offset;
        }
    }

    void OnMouseUp()
    {
        dragging = false;
    }
}
