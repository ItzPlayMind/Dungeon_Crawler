using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability_Display_Panel : MonoBehaviour
{
    public static Ability_Display_Panel instance;

    private void Awake()
    {
        instance = this;
    }

    public Ability_Slot_Prefab[] ability_Slot_Prefabs = new Ability_Slot_Prefab[3];

    public void Setup(Skill[] skills)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            ability_Slot_Prefabs[i].GetComponent<Image>().sprite = skills[i].icon;
        }
    }

    public void SetTimeForIndex(int index, float time)
    {
        ability_Slot_Prefabs[index].SetTime(time);
    }
}
