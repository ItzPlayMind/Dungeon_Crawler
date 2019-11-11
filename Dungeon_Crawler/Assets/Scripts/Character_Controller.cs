using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NetworkIdentity))]
public class Character_Controller : NetworkBehaviour
{
    public bool isRedTeam = false;
    NavMeshAgent agent;
    [SerializeField]private GameObject displayPrefab;
    [SerializeField] private FieldOfView fieldOfView;

    Character_Stats ownStats;

    [SerializeField] private Ability_Display ability_Display;

    Character_Animator animator;
    

    public void Setup(bool notSameTeam)
    {
        if (notSameTeam)
        {
            if (!isLocal)
            {
                fieldOfView.gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Character_Animator>();
        agent = GetComponent<NavMeshAgent>();
        ownStats = GetComponent<Character_Stats>();
    }

    Vector3 oldPos = Vector3.zero;
    Vector3 oldEuler = Vector3.zero;
    GameObject target;
    bool canAttack = true;

    public void Attack()
    {
        animator.PlayAnimation(Character_Animator.AnimationState.Attack);
        canAttack = false;
    }

    public void AttackAnimationFinished()
    {
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= ownStats.GetStat("Attack Range").value)
            {
                if (target.GetComponent<Character_Stats>().TakeDamage(ownStats.GetStat("Attack Damage").value))
                {
                    ownStats.GetStat("Gold").value += 100;
                    ownStats.AddXP(10);
                }
                SendAttackDamage(ownStats.GetStat("Attack Damage").value, target);
            }
        }
        canAttack = true;
    }

    public void SendAttackDamage(float damage, GameObject target)
    {
        JSONObject obj = new JSONObject();
        Debug.Log(target.GetComponent<NetworkIdentity>().ID);
        obj.AddField("id", target.GetComponent<NetworkIdentity>().ID.ToString());
        obj.AddField("damage", damage);
        NetworkManager.instance.Emit("damage", obj);
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
                    var dir = (target.transform.position - transform.position).normalized;
                    dir.y = 0;
                    transform.rotation = Quaternion.LookRotation(dir);
                    if (canAttack)
                        Attack();
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
                animator.PlayAnimation(Character_Animator.AnimationState.Walk);
                var obj = new JSONObject();
                obj.AddField("id", ID);
                obj.AddField("position", transform.position.convertToJson());
                obj.AddField("rotation", transform.eulerAngles.convertToJson());
                NetworkManager.instance.Emit("update position", obj);
            }
            else
            {
                animator.PlayAnimation(Character_Animator.AnimationState.Idle);
            }
            oldPos = transform.position;
            oldEuler = transform.eulerAngles;
        }
    }

    /*public void JumpTowards(Transform target, float initialAngle)
    {
        var rigid = GetComponent<Rigidbody>();

        Vector3 p = target.position;

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        // Fire!
        rigid.velocity = finalVelocity;

        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
    }*/

    
}
