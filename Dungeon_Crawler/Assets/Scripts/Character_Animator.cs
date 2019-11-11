using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Animator : NetworkBehaviour
{
    public enum AnimationState
    {
        Idle,
        Walk,
        Attack,
        Cast
    }

    [SerializeField] Animator animator;
   
    public void PlayAnimation(AnimationState state)
    {
        JSONObject obj = new JSONObject();
        switch (state)
        {
            case AnimationState.Idle:
                animator.SetBool("Walking", false);
                obj.AddField("state", "Idle");
                break;
            case AnimationState.Walk:
                animator.SetBool("Walking", true);
                obj.AddField("state", "Walk");
                break;
            case AnimationState.Attack:
                animator.SetTrigger("Attack");
                obj.AddField("state", "Attack");
                break;
            case AnimationState.Cast:
                animator.SetTrigger("Cast");
                obj.AddField("state", "Cast");
                break;
        }
        obj.AddField("id", ID);
        NetworkManager.instance.Emit("play animation", obj);
    }

    public void PlayAnimation(string name)
    {
        switch (name)
        {
            case "Idle":
                animator.SetBool("Walking", false);
                break;
            case "Walk":
                animator.SetBool("Walking", true);
                break;
            case "Attack":
                animator.SetTrigger("Attack");
                break;
            case "Cast":
                animator.SetTrigger("Cast");
                break;
        }
    }
}
