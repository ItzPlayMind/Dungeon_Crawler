using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Airborne : BaseEffect
{
    HealthBar bar;
    public Action<Vector3, GameObject> onGroundHit;

    public override void onEnable()
    {
        bar = GetComponent<Character_Stats>().healthbar;
        bar.SetEffectName("Airborne");
        if (GetComponent<NavMeshAgent>() != null)
            GetComponent<NavMeshAgent>().enabled = false;
        if (GetComponent<Character_Controller>() != null)
            GetComponent<Character_Controller>().enabled = false;
        if (GetComponent<Character_Ability>() != null)
            GetComponent<Character_Ability>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    bool onGround = true;

    public override void onUpdate()
    {
        bar.ToggleEffectDisplay(true);
        bar.SetEffectValue(duration / MaxDuration);
        duration = MaxDuration;
        if (Physics.Raycast(transform.position, Vector3.down, 0.1f, LayerMask.GetMask("Ground")))
        {
            if (!onGround)
            {
                onGround = true;
                onDisable();
                onGroundHit.Invoke(transform.position, gameObject);
                DestroyImmediate(this);

            }
            else
            {
                onGround = false;
            }
        }
    }

    public override void onDisable()
    {
        bar.ToggleEffectDisplay(false);
        if (GetComponent<NavMeshAgent>() != null)
            GetComponent<NavMeshAgent>().enabled = true;
        if (GetComponent<Character_Controller>() != null)
            GetComponent<Character_Controller>().enabled = true;
        if (GetComponent<Character_Ability>() != null)
            GetComponent<Character_Ability>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
