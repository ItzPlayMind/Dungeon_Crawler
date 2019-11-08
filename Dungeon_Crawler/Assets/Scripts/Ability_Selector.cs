using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability_Selector : MonoBehaviour
{
    #region Singleton
    public static Ability_Selector instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public Image[] selectedAbilitieImages;
    public List<Skill> allSkills = new List<Skill>();
    public GameObject presetAbility;
    public Transform abilityContent;
    public Button nextButton;

    private void Start()
    {
        foreach (var item in allSkills)
        {
            var a = Instantiate(presetAbility, abilityContent);
            a.GetComponent<AbilityBTN_Prefab>().Setup(item.Copy());
        }
    }

    public void Select(Skill skill)
    {
        for (int i = 0; i < NetworkManager.instance.abilitiesForSpawn.Length; i++)
        {
            if (NetworkManager.instance.abilitiesForSpawn[i] == null)
            {
                NetworkManager.instance.abilitiesForSpawn[i] = skill;
                break;
            }
        }
    }

    public void Remove(int index)
    {
        NetworkManager.instance.abilitiesForSpawn[index] = null;
    }

    private void Update()
    {
        bool isVisible = true;
        for (int i = 0; i < NetworkManager.instance.abilitiesForSpawn.Length; i++)
        {
            if (NetworkManager.instance.abilitiesForSpawn[i] != null)
            {
                selectedAbilitieImages[i].sprite = NetworkManager.instance.abilitiesForSpawn[i].icon;
            }
            else
            {
                isVisible = false;
            }
        }
        nextButton.gameObject.SetActive(isVisible);
    }
}
