using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Panel_Prefab : MonoBehaviour
{
    public Item item;
    public Image iconSprite;
    
    public void Setup(Item item)
    {
        this.item = item;
        iconSprite.sprite = item.icon;
    }

    public void Buy()
    {
        var stats = NetworkManager.instance.Player.GetComponent<Character_Stats>();
        if (stats.CanAddItem() && stats.GetStat("Gold").value >= item.cost)
        {
            stats.GetStat("Gold").value -= item.cost;
            stats.AddItem(item);
            JSONObject obj = new JSONObject();
            obj.AddField("id", NetworkManager.instance.Player.GetComponent<NetworkIdentity>().ID.RemoveQuotations());
            int i = 1;
            foreach (var item in stats.Items)
            {
                if (item != null)
                    obj.AddField("item" + i, item.name);
                else
                    obj.AddField("item" + i, "");
                i++;
            }
            NetworkManager.instance.Emit("change item", obj);
        }
    }
}
