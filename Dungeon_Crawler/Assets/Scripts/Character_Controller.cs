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
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    Vector3 oldPos = Vector3.zero;

    void Update()
    {
        if (isLocal)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Destroy(Instantiate(displayPrefab, hit.point + Vector3.up * 0.1f, Quaternion.identity), 1f);
                    //fieldOfView.SetAimDirection((hit.point + Vector3.up * 0.1f)-transform.position);
                    agent.SetDestination(hit.point);
                }
            }
            if (oldPos != transform.position)
            {
                var obj = new JSONObject();
                obj.AddField("id", ID);
                obj.AddField("position", transform.position.convertToJson());
                NetworkManager.instance.Emit("update position", obj);
            }
            oldPos = transform.position;
        }
    }

    
}
