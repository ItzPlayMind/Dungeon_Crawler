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

    class Comparer : IComparer<Skill>
    {
        public int Compare(Skill x, Skill y)
        {
            return x.minLevel.CompareTo(y.minLevel);
        }
    }

    public Image[] selectedAbilitieImages;
    public Image[] abilityPointImages;
    public List<Skill> allSkills = new List<Skill>();
    public GameObject presetAbility;
    public Transform abilityContent;
    public Button nextButton;
    public int MaxAbilityPoints;
    int currentAbilityPoints;

    private void Start()
    {
        allSkills.Sort(new Comparer());
        foreach (var item in allSkills)
        {
            var a = Instantiate(presetAbility, abilityContent);
            a.GetComponent<AbilityBTN_Prefab>().Setup(item.Copy());
        }
    }

    public void Select(Skill skill)
    {
        if (currentAbilityPoints + skill.abilityPoints <= MaxAbilityPoints)
        {
            for (int i = 0; i < NetworkManager.instance.abilitiesForSpawn.Length; i++)
            {
                if (NetworkManager.instance.abilitiesForSpawn[i] == null)
                {
                    NetworkManager.instance.abilitiesForSpawn[i] = skill;
                    currentAbilityPoints += skill.abilityPoints;
                    break;
                }
            }
        }
    }

    public void Remove(int index)
    {
        currentAbilityPoints -= NetworkManager.instance.abilitiesForSpawn[index].abilityPoints;
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
                selectedAbilitieImages[i].sprite = null;
                isVisible = false;
            }
        }
        for (int i = 0; i < MaxAbilityPoints; i++)
        {
            if(currentAbilityPoints > i)
                abilityPointImages[i].color = new Color(0, 0, 1, 1);
            else
                abilityPointImages[i].color = new Color(1, 1, 1, 1);
        }
        nextButton.gameObject.SetActive(isVisible);
    }
}
