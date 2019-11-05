using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkScriptableObject : ScriptableObject
{
    [SerializeField] private string id;
    public string ID { get => id; }

    public virtual NetworkScriptableObject Copy()
    {
        NetworkScriptableObject obj = new NetworkScriptableObject();
        obj.id = id;
        return obj;
    }
}
