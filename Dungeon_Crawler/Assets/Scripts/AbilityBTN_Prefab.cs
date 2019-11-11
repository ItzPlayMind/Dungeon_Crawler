using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityBTN_Prefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Ability_Description_Panel.instance.Display(skill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Ability_Description_Panel.instance.DontDisplay();
    }
}
