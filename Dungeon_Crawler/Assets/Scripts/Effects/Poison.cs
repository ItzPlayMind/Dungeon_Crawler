using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Poison : BaseEffect
{
    HealthBar bar;
    public float damage;
    Character_Stats stats;

    public override void onEnable()
    {
        stats = GetComponent<Character_Stats>();
        bar = stats.healthbar;
        bar.SetEffectName("Poison");
    }

    float timer = 0;

    public override void onUpdate()
    {
        bar.ToggleEffectDisplay(true);
        bar.SetEffectValue(duration / MaxDuration);
        timer += Time.deltaTime;
        if (timer >= 0.25f)
        {
            stats.TakeDamage(damage);
            timer = 0;
        }
    }

    public override void onDisable()
    {
        bar.ToggleEffectDisplay(false);

    }
}
