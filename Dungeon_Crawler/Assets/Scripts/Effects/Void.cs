using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Void : BaseEffect
{
    HealthBar bar;
    public float damage;

    public override void onEnable()
    {
        bar = GetComponent<Character_Stats>().healthbar;
        bar.SetEffectName("Void");
        
    }

    float timer = 0;

    public override void onUpdate()
    {
        bar.ToggleEffectDisplay(true);
        bar.SetEffectValue(duration / MaxDuration);
        timer += Time.deltaTime;
        if(timer > 1)
        {
            GetComponent<Character_Stats>().TakeDamage(damage);
            timer = 0;
        }
    }

    public override void onDisable()
    {
        bar.ToggleEffectDisplay(false);
    }
}
