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
    public float cooldown;
    public float level;
    float countdown;
    [Header("On Use Method")]
    public string onUseMethodName;
    public SkillData onUseData;

    [Header("On Hit Method")]
    public string onHitMethodName;
    public SkillData onHitData;

    [Header("On Display Method")]
    public string onDisplayMethodName;
    public SkillData onDisplayData;

    [Header("UI")]
    public Sprite icon;

    public bool CanUse { get => countdown <= 0; }
    public float getTime { get => countdown; }

    public Vector3 Display(Vector3 mousePosition, Vector3 dir, Vector3 center, Ability_Display display)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(a => a.GetType("ScriptSkills", false) != null);
        return (Vector3)assembly.GetType("ScriptSkills").GetMethod(onDisplayMethodName).Invoke(null, new object[] { onDisplayData, mousePosition, dir, center, display });
    }

    public void Use(string id, Vector3 position)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(a => a.GetType("ScriptSkills", false) != null);
        assembly.GetType("ScriptSkills").GetMethod(onUseMethodName).Invoke(null, new object[] { onUseData, id, position,this});
        countdown = cooldown;
    }

    public void OnHit(GameObject target, GameObject user)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly assembly = assemblies.FirstOrDefault(a => a.GetType("ScriptSkills", false) != null);
        assembly.GetType("ScriptSkills").GetMethod(onHitMethodName).Invoke(null, new object[] { onUseData, user, target, this });
    }

    public void Cooldown()
    {
        if (countdown >= 0)
        {
            countdown -= Time.deltaTime;
        }
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
        skill.cooldown = cooldown;
        skill.countdown = countdown;
        skill.icon = icon;
        skill.onDisplayMethodName = onDisplayMethodName;
        skill.onDisplayData = onDisplayData;
        return skill;
    }
}
