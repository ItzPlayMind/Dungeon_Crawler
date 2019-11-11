using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject Bar;
    [SerializeField] private Image healthBarFill;

    [SerializeField] private Image levelBarFill;
    [SerializeField] private Image effectBarFill;
    [SerializeField] TMPro.TextMeshProUGUI effectNameText;
    [SerializeField] TMPro.TextMeshProUGUI levelText;
    [SerializeField] GameObject effectDisplay;
    
    float healthValue;
    float levelValue;
    float effectValue;
    string effectName;

    public void SetLevel(int lvl)
    {
        levelText.text = lvl + "";
    }

    public void SetHealthValue(float amount)
    {
        healthValue = Mathf.Clamp01(amount);
    }

    public void SetLevelValue(float amount)
    {
        levelValue = Mathf.Clamp01(amount);
    }

    public void SetEffectValue(float amount)
    {
        effectValue = Mathf.Clamp01(amount);
    }

    public void SetEffectName(string name)
    {
        this.effectName = name;
    }

    public void ToggleEffectDisplay(bool value)
    {
        effectDisplay.SetActive(value);
    }

    private void Update()
    {
        healthBarFill.fillAmount = healthValue;
        levelBarFill.fillAmount = levelValue;
        effectBarFill.fillAmount = effectValue;
        effectNameText.text = effectName;
        Vector3 dir = (Camera.main.transform.position - Bar.transform.position).normalized;
        dir.x = 0;
        var lookRotation = Quaternion.LookRotation(dir);
        Bar.transform.rotation = lookRotation;
    }
}
