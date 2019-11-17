using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Items : NetworkBehaviour
{
    Character_Stats ownStats;
    void Start()
    {
        ownStats = GetComponent<Character_Stats>();
    }

    Item activeItem;
    int activeIndex;

    void Update()
    {
        if (isLocal)
        {
            for (int i = 0; i < ownStats.Items.Length; i++)
            {
                ownStats.Items[i]?.Passive(gameObject);
            }

            Vector3 mousePos = mouseWorldPosition();
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                activeItem = ownStats.Items[0];
                activeIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                activeItem = ownStats.Items[1];
                activeIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                activeItem = ownStats.Items[2];
                activeIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                activeItem = ownStats.Items[3];
                activeIndex = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                activeItem = ownStats.Items[4];
                activeIndex = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                activeItem = ownStats.Items[5];
                activeIndex = 5;
            }
            else if(activeItem != null)
            {
                activeItem?.Active(gameObject, mousePos, 0);
                JSONObject obj = new JSONObject();
                obj.AddField("id", ID);
                obj.AddField("itemIndex", activeIndex);
                obj.AddField("pos", mousePos.convertToJson());
                NetworkManager.instance.Emit("use item", obj);
                activeItem = null;
                activeIndex = -1;
            }
        }
    }

    Vector3 mouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.point;
        return Vector3.zero;
    }
}
