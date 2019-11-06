using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability_Slot_Prefab : MonoBehaviour
{
    [SerializeField] Image time_display;
    public void SetTime(float value)
    {
        time_display.fillAmount = value;
    }
}
