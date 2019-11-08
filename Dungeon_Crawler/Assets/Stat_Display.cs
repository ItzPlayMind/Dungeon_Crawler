using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat_Display : MonoBehaviour
{
    public TMPro.TextMeshProUGUI goldStatText;
    public Image[] itemSlots;

    Character_Stats playerStats;
    

    private void Update()
    {
        if (NetworkManager.instance.Player != null)
        {
            playerStats = NetworkManager.instance.Player.GetComponent<Character_Stats>();

            goldStatText.text = "Gold: " + playerStats.GetStat("Gold").value;
            for (int i = 0; i < playerStats.Items.Length; i++)
            {
                if (playerStats.Items[i] != null)
                    itemSlots[i].sprite = playerStats.Items[i].icon;
                else
                    itemSlots[i].sprite = null;
            }
        }
    }
}
