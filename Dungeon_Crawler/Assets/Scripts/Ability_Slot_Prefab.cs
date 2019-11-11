using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability_Slot_Prefab : MonoBehaviour
{
    [SerializeField] Image time_display;
    [SerializeField] GameObject usageDisplay;
    [SerializeField] TMPro.TextMeshProUGUI usageText;
    public void SetLevelCap(bool value)
    {
        if (!value)
        {
            time_display.fillAmount = 1;
        }
    }

    public void SetTime(float value)
    {
        time_display.fillAmount = value;
    }

    public void SetUsage(int i)
    {
        if(i > 0)
        {
            usageDisplay.SetActive(true);
            usageText.text = i.ToString();
        }
        else
        {
            usageDisplay.SetActive(false);
        }
    }
}
