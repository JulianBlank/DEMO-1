using UnityEngine;

public class Bogen_attack : MonoBehaviour
{
    private bool isequipped;
    private DungeonPlayerController dpc;

    void Update()
    {
        dpc = GetComponent<DungeonPlayerController>();
        if (dpc.HasAnyWeapon())
        {
            isequipped = true;
        }

        if (Input.GetMouseButtonDown(2) && isequipped == true)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            dpc.SetAttackBoxSize(mouseScreenPos.x, mouseScreenPos.y);

        }
    }
    
}
