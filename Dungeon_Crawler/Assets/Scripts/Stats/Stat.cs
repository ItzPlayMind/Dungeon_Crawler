using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Stats/New Stat")]
public class Stat : ScriptableObject
{
    public float value;
    float baseValue;
    public float BaseValue { get => baseValue; }

    public void Setup()
    {
        baseValue = value;
    }

    public Stat Copy()
    {
        Stat stat = new Stat();
        stat.value = value;
        stat.name = name;
        stat.Setup();
        return stat;
    }
}
