using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBTN_Prefab : MonoBehaviour
{
    public Skill skill;

    public void Setup(Skill skill)
    {
        this.skill = skill;
        GetComponent<Image>().sprite = skill.icon;
    }

    public void Select()
    {
        Ability_Selector.instance.Select(skill);
    }
}
