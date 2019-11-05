using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkIdentity : MonoBehaviour
{
    public string ID { get; private set; }
    public bool isLocal { get; private set; }

    public void setID(string id)
    {
        ID = id;
    }

    public void setIsLocal(bool isLocal)
    {
        this.isLocal = isLocal;
    }
}
