using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Skills/New Skill Data")]
public class SkillData : NetworkScriptableObject
{
    public GameObject[] objects;
    public float[] floats;
}
