using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability_Description_Panel : MonoBehaviour
{
    public static Ability_Description_Panel instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject Panel;
    [SerializeField] TMPro.TextMeshProUGUI titleText;
    [SerializeField] Image icon;
    [SerializeField] TMPro.TextMeshProUGUI descText;
    [SerializeField] TMPro.TextMeshProUGUI damageText;
    [SerializeField] TMPro.TextMeshProUGUI levelCapText;
    [SerializeField] TMPro.TextMeshProUGUI cooldownText;
    [SerializeField] TMPro.TextMeshProUGUI amountText;

    public void Display(Skill skill)
    {
        Panel.SetActive(true);
        titleText.text = skill.name;
        icon.sprite = skill.icon;
        descText.text = skill.description;
        damageText.text = skill.damage + "";
        levelCapText.text = skill.minLevel + "";
        cooldownText.text = skill.cooldown + "";
        if (skill.maxUsage < 0)
        {
            amountText.text = "1";
        }
        else
        {
            amountText.text = skill.maxUsage + "";
        }
    }

    private void Update()
    {
        if (Panel.activeSelf)
        {
            Panel.transform.position = Input.mousePosition + new Vector3(5,-5);
        }
    }

    public void DontDisplay()
    {
        Panel.SetActive(false);
    }
}
