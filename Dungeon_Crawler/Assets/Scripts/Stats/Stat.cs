using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Stats/New Stat")]
public class Stat : ScriptableObject
{
    [HideInInspector] public float MaxValue;
    public float value;
    float baseValue;
    public float BaseValue { get => baseValue; }

    public void Setup()
    {
        baseValue = value;
        MaxValue = value;
    }

    public Stat Copy()
    {
        Stat stat = new Stat();
        stat.value = value;
        stat.MaxValue = MaxValue;
        stat.name = name;
        stat.Setup();
        return stat;
    }
}
