using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject
{
    public float cost;
    public List<Stat> addStats = new List<Stat>();

    [Header("Passiv")]
    public string passivMethodName;

    [Header("Active")]
    public string activeMethodName;

    [Header("Icon")]
    public Sprite icon;

    public void Setup(Character_Stats stats)
    {
        for (int i = 0; i < addStats.Count; i++)
        {
            addStats[i] = addStats[i].Copy();
        }
        foreach (var item in addStats)
        {
            if(stats.GetStat(item.name) != null)
                stats.GetStat(item.name).value += item.value;
        }
    }

    public void Remove(Character_Stats stats)
    {
        foreach (var item in addStats)
        {
            if (stats.GetStat(item.name) != null)
                stats.GetStat(item.name).value -= item.value;
        }
    }

    public void Passive(GameObject holder)
    {
        if (passivMethodName != "")
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly = assemblies.FirstOrDefault(a => a.GetType("ScriptItems", false) != null);
            assembly.GetType("ScriptItems").GetMethod(passivMethodName).Invoke(null, new object[] { this, holder });
        }
    }

    public Vector3 Active(GameObject holder, Vector3 mousePosition)
    {
        if (activeMethodName != "")
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly = assemblies.FirstOrDefault(a => a.GetType("ScriptItems", false) != null);
            return (Vector3)assembly.GetType("ScriptItems").GetMethod(passivMethodName).Invoke(null, new object[] { this, holder, mousePosition });
        }
        return mousePosition;
    }

    public Item Copy()
    {
        Item item = new Item();
        List<Stat> s = new List<Stat>();
        foreach (var i in addStats)
        {
            s.Add(i.Copy());
        }
        item.addStats = s;
        item.passivMethodName = passivMethodName;
        item.activeMethodName = activeMethodName;
        item.name = name;
        item.icon = icon;
        item.cost = cost;
        return item;
    }
}
