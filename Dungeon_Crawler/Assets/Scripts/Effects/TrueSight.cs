using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TrueSight : BaseEffect
{
    HealthBar bar;

    GameObject gb = null;
    public Material mat;

    public override void onEnable()
    {
        bar = GetComponent<Character_Stats>().healthbar;
        bar.SetEffectName("True Sight");
        gb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gb.GetComponent<Collider>().enabled = false;
        gb.GetComponent<Renderer>().material = mat;
        gb.transform.SetParent(transform);
        gb.transform.localPosition = Vector3.zero;
        gb.transform.localScale = new Vector3(10, 0.1f, 10);
        gb.layer = LayerMask.NameToLayer("Mask");
    }

    public override void onUpdate()
    {
        bar.ToggleEffectDisplay(true);
        bar.SetEffectValue(duration / MaxDuration);
    }

    public override void onDisable()
    {
        bar.ToggleEffectDisplay(false);
        Destroy(gb);
    }
}
