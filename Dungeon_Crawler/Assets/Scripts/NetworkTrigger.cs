using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTrigger : MonoBehaviour
{
    public GameObject user;
    public Action<GameObject,GameObject,Vector3> onHit;
    public Action<GameObject, GameObject, Vector3> onStay;
    public Action<GameObject, GameObject, Vector3> onExit;
    public bool destroyOnTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        var identity = other.GetComponent<NetworkIdentity>();
        if (identity != null)
        {
            if (identity.ID != user.GetComponent<NetworkIdentity>().ID)
            {
                onHit(other.gameObject, user,transform.position);
                if(destroyOnTrigger)
                    Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var identity = other.GetComponent<NetworkIdentity>();
        if (identity != null)
        {
            if (identity.ID != user.GetComponent<NetworkIdentity>().ID)
            {
                onExit(other.gameObject, user, transform.position);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var identity = other.GetComponent<NetworkIdentity>();
        if (identity != null)
        {
            if (identity.ID != user.GetComponent<NetworkIdentity>().ID)
            {
                onStay(other.gameObject, user, transform.position);
            }
        }
    }
}
