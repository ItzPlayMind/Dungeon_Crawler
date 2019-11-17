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

    public static object ClickCircleAttack(SkillData data, Vector3 mousePos, Vector3 dir, Vector3 center, Ability_Display display)
    {
        display.DisplayClickCircleAttack(center, mousePos + Vector3.up * 0.11f, data.floats[0]);
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
        obj.GetComponent<Flying_Object>().speed  = skill.onDisplayData.floats[0]/ obj.GetComponent<Flying_Object>().destroyTime* 60;
        var trigger = obj.GetComponent<NetworkTrigger>();
        trigger.onHit += skill.OnHit;
        trigger.user = user;
        user.GetComponent<Character_Animator>().PlayAnimation(Character_Animator.AnimationState.Cast);
        /*
         
         * */
    }

    public static void FlyingKnifeHit(SkillData data, GameObject user, GameObject target, Vector3 initialPos, Skill skill)
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
        user.GetComponent<Rigidbody>().JumpTowards(position, data.floats[0]);
        //user.GetComponent<Character_Controller>().JumpTowards(position, data.floats[0]);
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
                if (user.GetComponent<Character_Controller>().DealDamage(skill.damage, item.gameObject))
                {
                    user.GetComponent<Character_Stats>().GetStat("Gold").value += 100;
                }
                var stun = item.gameObject.AddComponent<Airborne>();
                stun.MaxDuration = 2f;
                stun.onGroundHit += OnGroundHit;
                stun.Setup();
                item.GetComponent<Rigidbody>().velocity = Vector3.up * 10f;
            }
        }
    }

    public static void EagleVisionUse(SkillData data, string ID, Vector3 position, Skill skill)
    {
        GameObject user = NetworkManager.instance.GetPlayerByID(ID);
        if (user.GetComponent<NetworkIdentity>().isLocal)
        {
            CastTime.instance.Set(1f, "Casting Eagle Vision", OnEagleVisionCast, user, data, skill);
        }
        else
        {
            NetworkTimer.Create(1f, OnEagleVisionCast, user, data, skill);
        }
    }

    public static void OnEagleVisionCast(object[] objects)
    {
        GameObject user = (GameObject)objects[0];
        SkillData data = (SkillData)objects[1];
        Skill skill = (Skill)objects[2];

        user.GetComponent<Character_Animator>().PlayAnimation(Character_Animator.AnimationState.Cast);
        var teamUser = user.GetComponent<Character_Controller>().isRedTeam;
        var yourTeam = NetworkManager.instance.Player.GetComponent<Character_Controller>().isRedTeam;

        var obj = GameObject.Instantiate(data.objects[0], user.transform.position + Vector3.up, user.transform.rotation);
        obj.GetComponent<Flying_Object>().speed = skill.onDisplayData.floats[0] / obj.GetComponent<Flying_Object>().destroyTime * 60;
        var trigger = obj.GetComponent<NetworkTrigger>();
        trigger.onHit += skill.OnHit;
        trigger.user = user;

        if (teamUser != yourTeam)
        {
            obj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public static void EagleVisionHit(SkillData data, GameObject user, GameObject target, Vector3 initialPos, Skill skill)
    {
        Debug.Log(user.GetComponent<NetworkIdentity>().ID);
        var stun = target.AddComponent<TrueSight>();
        stun.MaxDuration = 3f;
        stun.mat = data.materials[0];
        stun.Setup();
    }

    public static void EarthWaveUse(SkillData data, string ID, Vector3 position, Skill skill)
    {

        GameObject user = NetworkManager.instance.GetPlayerByID(ID);
        var obj = GameObject.Instantiate(data.objects[0], user.transform.position + Vector3.up, user.transform.rotation);
        obj.GetComponent<Flying_Object>().speed = skill.onDisplayData.floats[0] / obj.GetComponent<Flying_Object>().destroyTime * 60;
        var trigger = obj.GetComponent<NetworkTrigger>();
        trigger.onHit += skill.OnHit;
        trigger.user = user;
        user.GetComponent<Character_Animator>().PlayAnimation(Character_Animator.AnimationState.Cast);
    }

    public static void EarthWaveHit(SkillData data, GameObject user, GameObject target, Vector3 initialPos, Skill skill)
    {
        if (user.GetComponent<Character_Controller>().DealDamage(skill.damage,target))
        {
            user.GetComponent<Character_Stats>().GetStat("Gold").value += 100;
        }
        var stun = target.gameObject.AddComponent<Airborne>();
        stun.MaxDuration = 2f;
        stun.onGroundHit += OnEarthWaveGroundHit;
        stun.Setup();
        target.GetComponent<Rigidbody>().velocity = Vector3.up * 10f;
    }

    public static void OnEarthWaveGroundHit(Vector3 pos, GameObject player)
    {
        var stun = player.AddComponent<TrueSight>();
        stun.MaxDuration = 5f;
        stun.Setup();
    }

    public static void PoisonBombsUse(SkillData data, string ID, Vector3 position, Skill skill)
    {

        GameObject user = NetworkManager.instance.GetPlayerByID(ID);
        
        var obj = GameObject.Instantiate(data.objects[0], user.transform.position + Vector3.up, user.transform.rotation);
        if (!user.GetComponent<NetworkIdentity>().isLocal)
        {
            obj.transform.GetChild(0).gameObject.SetActive(false);
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                obj.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("DontRender");
            }
        }
        var trigger = obj.GetComponent<NetworkTrigger>();
        trigger.onHit += skill.OnHit;
        trigger.user = user;
        user.GetComponent<Character_Animator>().PlayAnimation(Character_Animator.AnimationState.Cast);
        obj.GetComponent<Rigidbody>().JumpTowards(position, 75);
    }

    public static void PoisonBombsHit(SkillData data, GameObject user, GameObject target, Vector3 initialPos, Skill skill)
    {
        GameObject.Destroy(GameObject.Instantiate(data.objects[1], initialPos, Quaternion.identity), 5f);

        var stun = target.gameObject.AddComponent<Poison>();
        stun.MaxDuration = 2f;
        stun.damage = skill.damage;
        stun.Setup();

        var sight = target.gameObject.AddComponent<TrueSight>();
        sight.MaxDuration = 2f;
        sight.Setup();
    }

    public static void BlackHoleUse(SkillData data, string ID, Vector3 position, Skill skill)
    {

        GameObject user = NetworkManager.instance.GetPlayerByID(ID);
        var obj = GameObject.Instantiate(data.objects[0], position + Vector3.up, user.transform.rotation);
        if (!user.GetComponent<NetworkIdentity>().isLocal)
        {
            obj.transform.GetChild(0).gameObject.SetActive(false);
        }
        var trigger = obj.GetComponent<NetworkTrigger>();
        trigger.onHit += skill.OnHit;
        trigger.onStay += BlackHoleStay;
        trigger.user = user;
        GameObject.Destroy(obj, 10f);
        user.GetComponent<Character_Animator>().PlayAnimation(Character_Animator.AnimationState.Cast);
        //obj.GetComponent<Rigidbody>().JumpTowards(position, 45);
    }

    public static void BlackHoleStay(GameObject target, GameObject user, Vector3 initialPos)
    {
        Vector3 dir = (initialPos - target.transform.position).normalized;
        dir.y = 0;
        target.transform.position += dir*Time.deltaTime * 7f;
        var voidStun = target.GetComponent<Void>();
        voidStun.SetDuration(voidStun.MaxDuration);
    }

    public static void BlackHoleHit(SkillData data, GameObject user, GameObject target, Vector3 initialPos, Skill skill)
    {
        var stun = target.AddComponent<Void>();
        stun.damage = skill.damage;
        stun.MaxDuration = 5;
        stun.Setup();
    }

    public static void TeleportUse(SkillData data, string ID, Vector3 position, Skill skill)
    {
        GameObject user = NetworkManager.instance.GetPlayerByID(ID);
        if (user.GetComponent<NetworkIdentity>().isLocal)
        {
            CastTime.instance.Set(5f, "Casting Teleport", OnTeleportCast, user, position);
        }
        else
        {
            NetworkTimer.Create(5f, OnTeleportCast, user, position);
        }
    }

    public static void OnTeleportCast(object[] objects)
    {
        GameObject user = (GameObject)objects[0];
        Vector3 position = (Vector3)objects[1];
        var stun = user.AddComponent<Stunned>();
        stun.MaxDuration = 1f;
        stun.Setup();
        user.transform.position = position + Vector3.up;
    }

    public static void TotemUse(SkillData data, string ID, Vector3 position, Skill skill)
    {
        var user = NetworkManager.instance.GetPlayerByID(ID).GetComponent<Character_Controller>();
        var local = NetworkManager.instance.Player.GetComponent<Character_Controller>();
        var gb = GameObject.Instantiate(data.objects[0], position, Quaternion.identity);
        if(user.isRedTeam != local.isRedTeam)
        {
            gb.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
