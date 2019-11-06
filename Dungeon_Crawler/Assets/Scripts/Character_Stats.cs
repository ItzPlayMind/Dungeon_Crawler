using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Stats : NetworkBehaviour
{
    [SerializeField] List<Stat> stats = new List<Stat>();
    public HealthBar healthbar; 
    
    private void Start()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i] = (Stat)stats[i].Copy();
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

    public void TakeDamage(float value)
    {
        Debug.Log(gameObject.name + " took " + value + " Damage!");
        var health = GetStat("Health");
        health.value = Mathf.Max(health.value - value,0);
        healthbar.SetHealthValue(health.value / health.BaseValue);
        if (health.value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
