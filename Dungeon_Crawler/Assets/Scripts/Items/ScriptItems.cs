using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptItems
{
    public static void HealingPotionActive(Item item, int index, GameObject holder, Vector3 mousePosition)
    {
        var heal = holder.AddComponent<Healing>();
        heal.MaxDuration = 10;
        heal.healAmount = 25;
        heal.Setup();
        holder.GetComponent<Character_Stats>().RemoveItem(index);
        
    }
}
