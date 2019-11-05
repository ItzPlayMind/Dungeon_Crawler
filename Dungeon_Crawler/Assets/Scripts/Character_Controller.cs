using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NetworkIdentity))]
public class Character_Controller : NetworkBehaviour
{
    NavMeshAgent agent;
    [SerializeField]private GameObject displayPrefab;
    [SerializeField] private FieldOfView fieldOfView;

    Character_Stats ownStats;

    [SerializeField] private Ability_Display ability_Display;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ownStats = GetComponent<Character_Stats>();
        if (!isLocal)
        {
            fieldOfView.gameObject.SetActive(false);
        }
    }

    Vector3 oldPos = Vector3.zero;
    Vector3 oldEuler = Vector3.zero;
    GameObject target;
    bool canAttack = true;

    public IEnumerator Attack()
    {
        canAttack = false;
        yield return new WaitForSeconds(1/ownStats.GetStat("Attack Speed").value);
        if(target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= ownStats.GetStat("Attack Range").value)
            {
                target.GetComponent<Character_Stats>().TakeDamage(ownStats.GetStat("Attack Damage").value);
                JSONObject obj = new JSONObject();
                Debug.Log(target.GetComponent<NetworkIdentity>().ID);
                obj.AddField("id", target.GetComponent<NetworkIdentity>().ID.ToString());
                obj.AddField("damage", ownStats.GetStat("Attack Damage").value);
                obj.Bake();
                NetworkManager.instance.Emit("damage", obj);
            }
        }
        canAttack = true;
    }

    void Update()
    {
        if (isLocal)
        {
            if(target != null)
            {
                agent.SetDestination(target.transform.position);
                if(Vector3.Distance(target.transform.position, transform.position) <= ownStats.GetStat("Attack Range").value)
                {
                    if(canAttack)
                        StartCoroutine(Attack());
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var stats = hit.collider.GetComponent<Character_Stats>();
                    if(stats != null && stats != ownStats)
                    {
                        float attackRange = ownStats.GetStat("Attack Range").value;
                        agent.stoppingDistance = attackRange-0.15f;
                        target = stats.gameObject;
                    }
                    else
                    {
                        target = null;
                        agent.stoppingDistance = 0.1f;
                        Destroy(Instantiate(displayPrefab, hit.point + Vector3.up * 0.1f, Quaternion.identity), 1f);
                        agent.SetDestination(hit.point);
                    }
                }
            }
            if (oldPos != transform.position || oldEuler != transform.eulerAngles)
            {
                var obj = new JSONObject();
                obj.AddField("id", ID);
                obj.AddField("position", transform.position.convertToJson());
                obj.AddField("rotation", transform.eulerAngles.convertToJson());
                NetworkManager.instance.Emit("update position", obj);
            }
            oldPos = transform.position;
            oldEuler = transform.eulerAngles;
        }
    }

    
}
