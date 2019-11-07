using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Character_Ability : NetworkBehaviour
{
    public Ability_Display display;
    Character_Stats ownStats;
    NavMeshAgent agent;

    public Skill[] skills = new Skill[3];

    // Start is called before the first frame update
    void Start()
    {

        ownStats = GetComponent<Character_Stats>();
        agent = GetComponent<NavMeshAgent>();
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i] = (Skill)skills[i].Copy();
        }

        if (isLocal)
        {
            Ability_Display_Panel.instance.Setup(skills);
        }
        
    }

    Skill activeSkill = null;

    Vector3 destinationPos = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (isLocal)
        {
            Vector3 dir = (mouseWorldPosition() - transform.position);
            dir.y = 0;
            if (Input.GetKey(KeyCode.Q))
            {
                if (skills[0].CanUse)
                {
                    //display.DisplayRangedAttack(transform.position + Vector3.up * 0.1f, dir.normalized, skills[0].range, 2f);
                    destinationPos = skills[0].Display(mouseWorldPosition(), dir.normalized, transform.position + Vector3.up * 0.1f, display);
                    activeSkill = skills[0];
                    agent.SetDestination(transform.position);
                    transform.rotation = Quaternion.LookRotation(dir.normalized);
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                if (skills[1].CanUse)
                {
                    destinationPos = skills[1].Display(mouseWorldPosition(), dir.normalized, transform.position + Vector3.up * 0.1f, display);
                    activeSkill = skills[1];
                    agent.SetDestination(transform.position);
                    transform.rotation = Quaternion.LookRotation(dir.normalized);
                }
            }
            else if (Input.GetKey(KeyCode.E))
            {
                if (skills[2].CanUse)
                {
                    destinationPos = skills[2].Display(mouseWorldPosition(), dir.normalized, transform.position + Vector3.up * 0.1f, display);
                    activeSkill = skills[2];
                    agent.SetDestination(transform.position);
                    transform.rotation = Quaternion.LookRotation(dir.normalized);
                }
            }
            else if (Input.GetKey(KeyCode.C))
            {
                display.DisplayCircleClickAttack(transform.position + Vector3.up * 0.1f, ownStats.GetStat("Attack Range").value*2);
            }
            else
            {
                if (activeSkill != null)
                {
                    activeSkill.Use(ID, destinationPos);
                    JSONObject Jobj = new JSONObject();
                    Jobj.AddField("id", ID);
                    Jobj.AddField("abilityID", activeSkill.name.RemoveQuotations());
                    Jobj.AddField("position", destinationPos.convertToJson());
                    NetworkManager.instance.Emit("use ability", Jobj);
                    destinationPos = Vector3.zero;
                }
                display.ResetDisplay();
                activeSkill = null;
            }
            int i = 0;
            foreach (var item in skills)
            {
                item.Cooldown();
                Ability_Display_Panel.instance.SetTimeForIndex(i,item.getTime/item.cooldown);
                i++;
            }
        }
    }

    Vector3 mouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.point;
        return Vector3.zero;
    }
}
