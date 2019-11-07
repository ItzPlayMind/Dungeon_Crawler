using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Stats : NetworkBehaviour
{
    [SerializeField] List<Stat> stats = new List<Stat>();
    [SerializeField] Item[] items = new Item[6];
    public HealthBar healthbar; 
    
    public Item[] Items { get => items; }

    public void SetItems(Item[] arr)
    {
        items = arr;
    }

    private void Start()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i] = stats[i].Copy();
        }
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i] = items[i].Copy();
                items[i].Setup(this);
                items[i].Passive(gameObject);
            }
        }
        healthbar.SetHealthValue(1);
        healthbar.SetManaValue(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Item_Shop.instance.GFX.SetActive(!Item_Shop.instance.GFX.activeSelf);
        }
    }

    public Stat GetStat(string name)
    {
        foreach (var item in stats)
        {
            if(item.name == name)
            {
                return item;
            }
        }
        return null;
    }

    public bool CanAddItem()
    {
        bool x = false;
        foreach (var item in items)
        {
            if (item == null)
                x = true;
        }
        return x;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item.Copy();
                items[i].Setup(this);
                items[i].Passive(gameObject);
                break;
            }
        }
    }

    public void RemoveItem(int index)
    {
        items[index].Remove(this);
        items[index] = null;
    }

    public void TakeDamage(float value)
    {
        Debug.Log(gameObject.name + " took " + value + " Damage!");
        var health = GetStat("Health");
        health.value = Mathf.Max(health.value - value,0);
        healthbar.SetHealthValue(health.value / health.BaseValue);
        if (health.value <= 0)
        {
            Destroy(gameObject);
            if(isLocal)
                NetworkManager.instance.Respawn(); 
        }
    }
}
