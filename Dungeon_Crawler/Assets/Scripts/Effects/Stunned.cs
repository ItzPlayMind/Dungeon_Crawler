using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stunned : BaseEffect
{
    HealthBar bar;
    
    public override void onEnable()
    {
        bar = GetComponent<Character_Stats>().healthbar;
        bar.SetEffectName("Stunned");
        if(GetComponent<NavMeshAgent>() != null)
            GetComponent<NavMeshAgent>().enabled = false;
        if (GetComponent<Character_Controller>() != null)
            GetComponent<Character_Controller>().enabled = false;
        if (GetComponent<Character_Ability>() != null)
            GetComponent<Character_Ability>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public override void onUpdate()
    {
        bar.ToggleEffectDisplay(true);
        bar.SetEffectValue(duration / MaxDuration);
    }

    public override void onDisable()
    {
        bar.ToggleEffectDisplay(false);
        if (GetComponent<NavMeshAgent>() != null)
        {
            GetComponent<NavMeshAgent>().enabled = true;
        }
        if (GetComponent<Character_Controller>() != null)
            GetComponent<Character_Controller>().enabled = true;
        if (GetComponent<Character_Ability>() != null)
            GetComponent<Character_Ability>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
