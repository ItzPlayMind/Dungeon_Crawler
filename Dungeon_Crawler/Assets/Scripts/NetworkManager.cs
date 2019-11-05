using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.Events;

public class NetworkManager : SocketIOComponent
{
    #region Singelton
    public static NetworkManager instance;

    public override void Awake()
    {
        base.Awake();
        instance = this;
    }
    #endregion

    public Action<GameObject> onSetup;

    public GameObject playerPrefab;
    
    public override void Start()
    {
        base.Start();
        On("setup", (SocketIOEvent e) =>
        {
            var behaviour = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<NetworkIdentity>();
            behaviour.setIsLocal(true);
            behaviour.setID(e.data["id"].ToString());
            Camera.main.GetComponent<Camera_Movement>().target = behaviour.transform;
            onSetup.Invoke(behaviour.gameObject);
        });

        On("new player setup", (SocketIOEvent e) =>
        {
            Debug.Log("New Player joined! " + e.data["position"].ToString());
            var behaviour = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<NetworkIdentity>();
            behaviour.setIsLocal(false);
            behaviour.setID(e.data["id"].ToString());
            Vector3 pos = GetVectorFromData(e.data["position"]);
            Debug.Log(pos);
            behaviour.transform.position = pos;
        });

        On("remove player", (SocketIOEvent e) =>
        {
            var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();
            
            foreach (var item in behaviours)
            {
                if(item.ID == e.data["id"].ToString())
                {
                    Destroy(item.gameObject);
                    break;
                }
            }
        });

        On("update position", (SocketIOEvent e) =>
        {
            var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();

            foreach (var item in behaviours)
            {
                if (item.ID == e.data["id"].ToString())
                {
                    item.transform.position = GetVectorFromData(e.data["position"]);
                    break;
                }
            }
        });
    }

    public Vector3 GetVectorFromData(JSONObject e)
    {
        float x = float.Parse(e["x"].ToString())/1000f;
        float y = float.Parse(e["y"].ToString())/1000f;
        float z = float.Parse(e["z"].ToString())/1000f;
        return new Vector3(x, y, z);
    }

    
}
