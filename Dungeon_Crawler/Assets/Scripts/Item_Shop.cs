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
}
