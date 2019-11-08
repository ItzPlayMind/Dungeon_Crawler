using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shop : MonoBehaviour
{
    #region Singleton
    public static Item_Shop instance;

    private void Awake()
    {
        instance = this;
    }
#endregion

    public List<Item> allItems = new List<Item>();
    public GameObject itemSlotPrefab;
    public Transform content;
    public GameObject GFX;

    void Start()
    {
        foreach (var item in allItems)
        {
            var prefab = Instantiate(itemSlotPrefab, content).GetComponent<Item_Panel_Prefab>();
            prefab.Setup(item);
        }
    }

    public void SellItem(int index)
    {
        var stats = NetworkManager.instance.Player.GetComponent<Character_Stats>();
        if (stats.Items[index] != null)
        {
            stats.GetStat("Gold").value += stats.Items[index].cost/2f;
            stats.RemoveItem(index);
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
