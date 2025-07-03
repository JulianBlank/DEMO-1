using UnityEngine;

public class Bogen_attack : MonoBehaviour
{
    public bool isequipped;
    //public DungeonPlayerController attackboxchanger;

    void Update()
    {
        //attackboxchanger = GetComponent<DungeonPlayercontroller>();

        if (Input.GetMouseButtonDown(2) && isequipped == true)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            //attackboxchanger.setattackbox(mouseScreenPos.x, mouseScreenPos.y);

        }
    }
    
}
