using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    }

    Skill activeSkill = null;

    // Update is called once per frame
    void Update()
    {
        if (isLocal)
        {
            Vector3 dir = (mouseWorldPosition() - transform.position);
            if (Input.GetKey(KeyCode.Alpha1))
            {
                display.DisplayRangedAttack(transform.position + Vector3.up*0.1f, dir.normalized, skills[0].range, 2f);
                activeSkill = skills[0];
                agent.SetDestination(transform.position);
                transform.rotation = Quaternion.LookRotation(dir.normalized);
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                display.DisplayCircleClickAttack(transform.position, 20f);
                activeSkill = skills[1];
                agent.SetDestination(transform.position);
                transform.rotation = Quaternion.LookRotation(dir.normalized);
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                display.DisplayCircleCircleAttack(transform.position, mouseWorldPosition(), 20f, 5f);
                activeSkill = skills[2];
                agent.SetDestination(transform.position);
                transform.rotation = Quaternion.LookRotation(dir.normalized);
            }
            else if (Input.GetKey(KeyCode.C))
            {
                display.DisplayCircleClickAttack(transform.position + Vector3.up * 0.1f, ownStats.GetStat("Attack Range").value*2);
            }
            else
            {
                if (activeSkill != null)
                {
                    
                    activeSkill.Use(ID);
                    JSONObject Jobj = new JSONObject();
                    Jobj.AddField("id", ID);
                    Jobj.AddField("abilityID", activeSkill.name.RemoveQuotations());
                    Jobj.AddField("position", mouseWorldPosition().convertToJson());
                    NetworkManager.instance.Emit("use ability", Jobj);
                }
                display.ResetDisplay();
                activeSkill = null;
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
