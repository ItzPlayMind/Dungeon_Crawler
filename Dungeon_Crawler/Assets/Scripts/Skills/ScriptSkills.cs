using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSkills
{
    public static void FlyingKnifeUse(SkillData data, string ID, Skill skill)
    {
        GameObject user = NetworkManager.instance.GetPlayerByID(ID);
        var obj = GameObject.Instantiate(data.objects[0], user.transform.position+Vector3.up, user.transform.rotation);
        obj.GetComponent<Flying_Object>().speed  = skill.range/0.5f*60;
        var trigger = obj.GetComponent<NetworkTrigger>();
        trigger.onHit += skill.OnHit;
        trigger.user = user;
    }

    public static void FlyingKnifeHit(SkillData data, GameObject user, GameObject target, Skill skill)
    {
        Debug.Log(user.GetComponent<NetworkIdentity>().ID);
        var stun = target.AddComponent<Stunned>();
        stun.MaxDuration = 1f;
        stun.Setup();
        if(target.GetComponent<Rigidbody>() != null)
        {
            Vector3 dir = (user.transform.position - target.transform.position);
            target.GetComponent<Rigidbody>().velocity = dir;
            Debug.Log("Pull! " + target.GetComponent<NetworkIdentity>().ID);
        }
    }
}
