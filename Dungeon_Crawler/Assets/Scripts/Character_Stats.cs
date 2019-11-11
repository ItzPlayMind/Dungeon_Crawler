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
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null && arr[i] != null)
            {
                AddItem(arr[i], i);
            }
            if (items[i] != null && arr[i] == null)
                RemoveItem(i);
        }
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
        healthbar.SetLevelValue(0);
        xp = GetStat("XP");
        if (isLocal)
        {
            healthbar.SetLevel(1);
            xp.MaxValue = 10;
            InvokeRepeating("AddXP", 1, 1);
        }
    }

    Stat xp;

    void AddXP()
    {
        xp.value += 1;
        if (xp.value >= xp.MaxValue)
        {
            var level = GetStat("Level");
            level.value++;
            xp.value = xp.value - xp.MaxValue;
            xp.MaxValue = 10 * level.value;
            healthbar.SetLevel((int)level.value);
        }
        healthbar.SetLevelValue(xp.value / xp.MaxValue);
        JSONObject obj = new JSONObject();
        obj.AddField("id", ID);
        obj.AddField("level", GetStat("Level").value);
        obj.AddField("xp", xp.value);
        NetworkManager.instance.Emit("change xp", obj);
    }

    public void SetToLevelWithXP(int level, int xp)
    {
        GetStat("Level").value = level;
        this.xp.MaxValue = 10 * level;
        this.xp.value = xp;
    }

    public void AddXP(int i)
    {
        if (xp == null)
            xp = GetStat("XP");
        xp.value += i;
        if (xp.value >= xp.MaxValue)
        {
            var level = GetStat("Level");
            level.value++;
            xp.value = xp.value - xp.MaxValue;
            xp.MaxValue = 10 * level.value;
            healthbar.SetLevel((int)level.value);
        }
        healthbar.SetLevelValue(xp.value / xp.MaxValue);
        if (isLocal)
        {
            JSONObject obj = new JSONObject();
            obj.AddField("id", ID);
            obj.AddField("level", GetStat("Level").value);
            obj.AddField("xp", xp.value);
            NetworkManager.instance.Emit("change xp", obj);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Item_Shop.instance.GFX.SetActive(true);
        }
    }

    public Stat GetStat(string name)
    {
        foreach (var item in stats)
        {
            if (item.name == name)
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

    public void AddItem(Item item, int i)
    {
        items[i] = item.Copy();
        items[i].Setup(this);
        items[i].Passive(gameObject);
    }

    public void RemoveItem(int index)
    {
        items[index].Remove(this);
        items[index] = null;
    }

    public bool TakeDamage(float value)
    {
        Debug.Log(gameObject.name + " took " + value + " Damage!");
        var health = GetStat("Health");
        health.value = Mathf.Max(health.value - value, 0);
        healthbar.SetHealthValue(health.value / health.MaxValue);
        if (health.value <= 0)
        {
            gameObject.SetActive(false);
            NetworkManager.instance.Respawn(gameObject);
            JSONObject obj = new JSONObject();
            obj.AddField("isRedTeam", !GetComponent<Character_Controller>().isRedTeam);
            NetworkManager.instance.Emit("kill player", obj);
            return true;
        }
        return false;
    }
}
