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
    public float range;
    public float cooldown;
    [Header("On Use Method")]
    public string onUseMethodName;
    public SkillData onUseData;

    [Header("On Hit Method")]
    public string onHitMethodName;
    public SkillData onHitData;

    public void Use(string id)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(a => a.GetType("ScriptSkills", false) != null);
        assembly.GetType("ScriptSkills").GetMethod(onUseMethodName).Invoke(null, new object[] { onUseData, id, this});
    }

    public void OnHit(GameObject target, GameObject user)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(a => a.GetType("ScriptSkills", false) != null);
        assembly.GetType("ScriptSkills").GetMethod(onHitMethodName).Invoke(null, new object[] { onUseData, user, target, this });
    }

    public Skill Copy()
    {
        Skill skill = new Skill();
        skill.name = name;
        skill.damage = damage;
        skill.manaCost = manaCost;
        skill.onUseData = onUseData;
        skill.onUseMethodName = onUseMethodName;
        skill.onHitData = onHitData;
        skill.onHitMethodName = onHitMethodName;
        skill.range = range;
        skill.cooldown = cooldown;
        return skill;
    }
}
