using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTrigger : MonoBehaviour
{
    public GameObject user;
    public Action<GameObject,GameObject> onHit;
    
    private void OnTriggerEnter(Collider other)
    {
        var identity = other.GetComponent<NetworkIdentity>();
        if (identity != null)
        {
            if (identity.ID != user.GetComponent<NetworkIdentity>().ID)
            {
                onHit(other.gameObject, user);
                
                Destroy(gameObject);
            }
        }
    }
}
