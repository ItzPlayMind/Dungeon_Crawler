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

    [SerializeField] TMPro.TMP_InputField input;
    public Transform redTeamSpawn;
    public Transform blueTeamSpawn;

    public GameObject Player { get; private set; }

    public Action<GameObject> onSetup;

    public GameObject playerPrefab;

    [HideInInspector]public Skill[] abilitiesForSpawn = new Skill[3];

    [SerializeField] TMPro.TextMeshProUGUI redScoreText;
    [SerializeField] TMPro.TextMeshProUGUI blueScoreText;

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
        input.text = "127.0.0.1";
        On("setup", (SocketIOEvent e) =>
        {
            var behaviour = Instantiate(playerPrefab, e.data["isRedTeam"].b ? redTeamSpawn.position : blueTeamSpawn.position, Quaternion.identity).GetComponent<NetworkIdentity>();
            /*if (!e.data["isRedTeam"].b)
            {
                redTeamSpawn.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                blueTeamSpawn.GetChild(0).gameObject.SetActive(false);
            }*/
            behaviour.setIsLocal(true);
            behaviour.setID(e.data["id"].ToString().RemoveQuotations());
            behaviour.GetComponent<Character_Controller>().isRedTeam = e.data["isRedTeam"].b;
            Player = behaviour.gameObject;
            Camera.main.GetComponent<Camera_Movement>().target = behaviour.transform;
            behaviour.GetComponent<Character_Ability>().skills = abilitiesForSpawn;
            JSONObject obj = new JSONObject();
            obj.AddField("id", behaviour.ID);
            for (int i = 0; i < abilitiesForSpawn.Length; i++)
            {
                obj.AddField("ability" + i, abilitiesForSpawn[i].name);
            }
            Emit("change abilities", obj);
            onSetup.Invoke(behaviour.gameObject);
        });

        On("new player setup", (SocketIOEvent e) =>
        {
            Debug.Log("New Player joined! " + e.data["isRedTeam"].b);
            var behaviour = Instantiate(playerPrefab, e.data["isRedTeam"].b ? redTeamSpawn.position : blueTeamSpawn.position, Quaternion.identity).GetComponent<NetworkIdentity>();
            behaviour.setIsLocal(false);
            behaviour.setID(e.data["id"].ToString().RemoveQuotations());
            var controller = behaviour.GetComponent<Character_Controller>();
            controller.isRedTeam = e.data["isRedTeam"].b;
            Debug.Log(controller.isRedTeam + " " + Player.GetComponent<Character_Controller>().isRedTeam);
            controller.Setup(Player.GetComponent<Character_Controller>().isRedTeam != controller.isRedTeam);
            Debug.Log(e.data["items"].list.Count);
            List<Item> items = new List<Item>();
            foreach (var item in e.data["items"].list)
            {
                items.Add(Item_Shop.instance.allItems.Find(x => x.name == item.ToString().RemoveQuotations())?.Copy());
            }
            List<Skill> skills = new List<Skill>();
            foreach (var skill in e.data["skills"].list)
            {
                skills.Add(Ability_Selector.instance.allSkills.Find(x => x.name == skill.ToString().RemoveQuotations())?.Copy());
            }
            behaviour.GetComponent<Character_Ability>().skills = skills.ToArray();
            Vector3 pos = GetVectorFromData(e.data["position"]);
            Vector3 rot = GetVectorFromData(e.data["rotation"]);
            behaviour.transform.position = pos;
            behaviour.transform.eulerAngles = rot;
        });

        On("remove player", (SocketIOEvent e) =>
        {
            var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();

            foreach (var item in behaviours)
            {
                if (item.ID == e.data["id"].ToString().RemoveQuotations())
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

            if (user != null)
            {
                var skills = user.GetComponent<Character_Ability>().skills;
                foreach (var item in skills)
                {
                    if (item.name == e.data["abilityID"].ToString().RemoveQuotations())
                    {
                        item.Use(e.data["id"].ToString().RemoveQuotations(), GetVectorFromData(e.data["position"]));
                        break;
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

        On("update score", (SocketIOEvent e) =>
        {
            redScoreText.text = e.data["redScore"].f + "";
            blueScoreText.text = e.data["blueScore"].f + "";
        });

        On("change item", (SocketIOEvent e) =>
        {
            var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();
            //Debug.Log(e.data["id"].ToString().RemoveQuotations() + " took " + e.data["damage"].ToString() + " Damage from " + e.data["attackerID"].ToString().RemoveQuotations());
            foreach (var entity in behaviours)
            {
                if (entity.ID == e.data["id"].ToString().RemoveQuotations())
                {
                    Debug.Log("Change Items from " + entity.ID);
                    List<Item> items = new List<Item>();
                    foreach (var item in e.data["items"].list)
                    {
                        items.Add(Item_Shop.instance.allItems.Find(x => x.name == item.ToString().RemoveQuotations())?.Copy());
                    }
                    Debug.Log(string.Join(",", items));
                    entity.GetComponent<Character_Stats>().SetItems(items.ToArray());
                    break;
                }
            }
        });

        On("change abilities", (SocketIOEvent e) =>
        {
            var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();
            //Debug.Log(e.data["id"].ToString().RemoveQuotations() + " took " + e.data["damage"].ToString() + " Damage from " + e.data["attackerID"].ToString().RemoveQuotations());
            foreach (var entity in behaviours)
            {
                if (entity.ID == e.data["id"].ToString().RemoveQuotations())
                {
                    Debug.Log("Change Abilities from " + entity.ID);
                    List<Skill> items = new List<Skill>();
                    foreach (var item in e.data["skills"].list)
                    {
                        items.Add(Ability_Selector.instance.allSkills.Find(x => x.name == item.ToString().RemoveQuotations())?.Copy());
                    }
                    Debug.Log(string.Join(",", items));
                    entity.GetComponent<Character_Ability>().skills = items.ToArray();
                    break;
                }
            }
        });
    }

    public void Respawn(GameObject gb)
    {
        var controller = gb.GetComponent<Character_Controller>();
        controller.transform.position = controller.isRedTeam ? redTeamSpawn.position : blueTeamSpawn.position;
        var health = controller.GetComponent<Character_Stats>().GetStat("Health");
        health.value = health.MaxValue;
        controller.GetComponent<Character_Stats>().healthbar.SetHealthValue(1);
        gb.SetActive(true);
        /*var controller = Player.GetComponent<Character_Controller>();
        Player = Instantiate(NetworkManager.instance.playerPrefab, controller.isRedTeam ? NetworkManager.instance.redTeamSpawn.position : NetworkManager.instance.blueTeamSpawn.position, Quaternion.identity);
        onSetup.Invoke(Player);*/
    }

    public override void Update()
    {
        base.Update();
        url = "ws://" + input.text + ":4567/socket.io/?EIO=4&transport=websocket";

    }

    public Vector3 GetVectorFromData(JSONObject e)
    {
        float x = float.Parse(e["x"].ToString()) / 1000f;
        float y = float.Parse(e["y"].ToString()) / 1000f;
        float z = float.Parse(e["z"].ToString()) / 1000f;
        return new Vector3(x, y, z);
    }

    public void BtnConnect()
    {
        Close();
        var behaviours = GameObject.FindObjectsOfType<NetworkIdentity>();
        foreach (var item in behaviours)
        {
            Destroy(item.gameObject);
        }
        Connect();

    }

}
