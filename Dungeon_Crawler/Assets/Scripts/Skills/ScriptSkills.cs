using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSkills
{
    public static object RangeAttackDisplay(SkillData data, Vector3 mousePos, Vector3 dir, Vector3 center,  Ability_Display display)
    {
        display.DisplayRangedAttack(center, dir, data.floats[0], data.floats[1]);
        return mousePos;
    }

    public static object CircleCircleAttack(SkillData data, Vector3 mousePos, Vector3 dir, Vector3 center, Ability_Display display)
    {
        display.DisplayCircleCircleAttack(center, mousePos+Vector3.up*0.11f, data.floats[0], data.floats[1]);
        if(Vector3.Distance(mousePos,center) > data.floats[0]/2f)
        {
            return center + dir.normalized * (data.floats[0]/2f);
        }
        return mousePos;
    }

    public static object CircleAttack(SkillData data, Vector3 mousePos, Vector3 dir, Vector3 center, Ability_Display display)
    {
        display.DisplayCircleClickAttack(center, data.floats[0]);
        return mousePos;
    }

    public static void FlyingKnifeUse(SkillData data, string ID, Vector3 position, Skill skill)
    {
        GameObject user = NetworkManager.instance.GetPlayerByID(ID);
        var obj = GameObject.Instantiate(data.objects[0], user.transform.position+Vector3.up, user.transform.rotation);
        obj.GetComponent<Flying_Object>().speed  = skill.onDisplayData.floats[0]/0.5f*60;
        var trigger = obj.GetComponent<NetworkTrigger>();
        trigger.onHit += skill.OnHit;
        trigger.user = user;

        /*
         
         * */
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

    public static void JumpDropUse(SkillData data, string ID, Vector3 position, Skill skill)
    {
        GameObject user = NetworkManager.instance.GetPlayerByID(ID);
        var stun = user.AddComponent<Airborne>();
        stun.MaxDuration = 2f;
        stun.onGroundHit += OnGroundHit;
        stun.Setup();
        user.GetComponent<Character_Controller>().JumpTowards(position, data.floats[0]);
    }

    public static void OnGroundHit(Vector3 pos, GameObject player)
    {
        var stun = player.AddComponent<Stunned>();
        stun.MaxDuration = 1f;
        stun.Setup();
    }

    public static void GroundShatterUse(SkillData data, string ID, Vector3 position, Skill skill)
    {
        GameObject user = NetworkManager.instance.GetPlayerByID(ID);

        GameObject.Destroy(GameObject.Instantiate(data.objects[0], user.transform.position, Quaternion.LookRotation(Vector3.up)), 1f);
        var objects = Physics.OverlapSphere(position, data.floats[0]/2f);
        foreach (var item in objects)
        {
            if(item.GetComponent<NetworkIdentity>() != null)
            {
                if (item.GetComponent<NetworkIdentity>().ID == ID)
                    continue;
                item.GetComponent<Character_Stats>().TakeDamage(skill.damage);
                var stun = item.gameObject.AddComponent<Airborne>();
                stun.MaxDuration = 2f;
                stun.onGroundHit += OnGroundHit;
                stun.Setup();
                item.GetComponent<Rigidbody>().velocity = Vector3.up * 10f;
            }
        }
    }
}
