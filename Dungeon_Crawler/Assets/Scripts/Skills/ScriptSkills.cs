using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSkills
{
    public static void SpawnObject(SkillData data, string ID)
    {
        var obj = GameObject.Instantiate(data.objects[0], NetworkManager.instance.GetPlayerByID(ID).transform.position+Vector3.up, NetworkManager.instance.GetPlayerByID(ID).transform.rotation);
    }
}
