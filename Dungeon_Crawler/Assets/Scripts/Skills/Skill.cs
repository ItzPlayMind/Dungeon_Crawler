using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/New Skill")]
public class Skill : ScriptableObject
{
    public float damage;
    public float manaCost;
    public SkillData data;
    public string methodName;

    public void Use(string id)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(a => a.GetType("ScriptSkills", false) != null);
        assembly.GetType("ScriptSkills").GetMethod(methodName).Invoke(null, new object[] { data, id});
    }

    public Skill Copy()
    {
        Skill skill = new Skill();
        skill.name = name;
        skill.damage = damage;
        skill.manaCost = manaCost;
        skill.data = data;
        skill.methodName = methodName;
        return skill;
    }
}
