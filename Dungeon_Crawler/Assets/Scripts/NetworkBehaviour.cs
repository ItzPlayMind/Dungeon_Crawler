using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkBehaviour : MonoBehaviour
{
    NetworkIdentity identity;
    private void Awake()
    {
        identity = GetComponent<NetworkIdentity>();
    }

    public string ID { get => identity.ID; }
    public bool isLocal { get => identity.isLocal; }
}
