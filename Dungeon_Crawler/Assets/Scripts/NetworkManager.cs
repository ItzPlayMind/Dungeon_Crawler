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




    public GameObject Player { get; private set; }

    public Action<GameObject> onSetup;

    public GameObject playerPrefab;

    public List<NetworkScriptableObject> networkScriptableObjects = new List<NetworkScriptableObject>();


    public GameObject GetPlayerByID(string userID)
    {
        var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();

        foreach (var item in behaviours)
        {
            if (item.ID == userID)
            {
                return item.gameObject;
            }
        }
        return null;
    }

    public override void Start()
    {
        base.Start();
        On("setup", (SocketIOEvent e) =>
        {
            var behaviour = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<NetworkIdentity>();
            behaviour.setIsLocal(true);
            behaviour.setID(e.data["id"].ToString().RemoveQuotations());
            Player = behaviour.gameObject;
            Camera.main.GetComponent<Camera_Movement>().target = behaviour.transform;
            onSetup.Invoke(behaviour.gameObject);
        });

        On("new player setup", (SocketIOEvent e) =>
        {
            Debug.Log("New Player joined! " + e.data["position"].ToString());
            var behaviour = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<NetworkIdentity>();
            behaviour.setIsLocal(false);
            behaviour.setID(e.data["id"].ToString().RemoveQuotations());
            Vector3 pos = GetVectorFromData(e.data["position"]);
            Vector3 rot = GetVectorFromData(e.data["rotation"]);
            Debug.Log(pos + " " + rot);
            behaviour.transform.position = pos;
            behaviour.transform.eulerAngles = rot;
        });

        On("remove player", (SocketIOEvent e) =>
        {
            var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();
            
            foreach (var item in behaviours)
            {
                if(item.ID == e.data["id"].ToString().RemoveQuotations())
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
                if (item.ID == e.data["id"].ToString().RemoveQuotations())
                {
                    item.transform.position = GetVectorFromData(e.data["position"]);
                    item.transform.eulerAngles = GetVectorFromData(e.data["rotation"]);
                    break;
                }
            }
        });

        On("use ability", (SocketIOEvent e) =>
        {
            var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();

            GameObject user = null;

            foreach (var item in behaviours)
            {
                if (item.ID == e.data["id"].ToString().RemoveQuotations())
                {
                    user = item.gameObject;
                    break;
                }
            }

            if(user != null)
            {
                var skills = user.GetComponent<Character_Ability>().skills;
                foreach (var item in skills)
                {
                    if (item.name == e.data["abilityID"].ToString().RemoveQuotations())
                    {
                        item.Use(e.data["id"].ToString().RemoveQuotations());
                    }
                }
            }
        });

        On("damage", (SocketIOEvent e) =>
        {
            var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();
            //Debug.Log(e.data["id"].ToString().RemoveQuotations() + " took " + e.data["damage"].ToString() + " Damage from " + e.data["attackerID"].ToString().RemoveQuotations());
            foreach (var item in behaviours)
            {
                if (item.ID == e.data["id"].ToString().RemoveQuotations())
                {
                    item.GetComponent<Character_Stats>().TakeDamage(e.data["damage"].f);
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
