using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    public float MaxDuration;
    protected float duration;

    public void SetDuration(float d)
    {
        duration = d;
    }

    public void Setup()
    {
        duration = MaxDuration;
        onEnable();
    }

    public virtual void onEnable()
    {

    }
    public virtual void onDisable()
    {

    }
    public virtual void onUpdate()
    {

    }

    private void Update()
    {
        duration -= Time.deltaTime;
        onUpdate();
        if(duration <= 0)
        {
            onDisable();
            DestroyImmediate(this);
        }
    }

}
