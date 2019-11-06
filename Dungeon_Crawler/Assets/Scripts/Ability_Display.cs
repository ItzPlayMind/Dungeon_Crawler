using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Display : MonoBehaviour
{
    public Material quadratMat;
    public Material circleMat;
    GameObject display;
    GameObject display2;

    public void ResetDisplay()
    {
        Destroy(display);
        Destroy(display2);
        display = null;
        display2 = null;
    }

    public void DisplayRangedAttack(Vector3 origin, Vector3 dir, float length, float width)
    {
        if (display == null)
        {
            display = GameObject.CreatePrimitive(PrimitiveType.Plane);
            display.layer = LayerMask.NameToLayer("BehindMask");
            DestroyImmediate(display.GetComponent<Collider>());
        }
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            display.transform.rotation = lookRotation;
        }
        display.transform.SetParent(transform);
        display.transform.position = origin + (dir * length / 2);
        display.transform.localScale = new Vector3(width / 10, 0, length / 10);
        display.GetComponent<MeshRenderer>().material = quadratMat;
    }

    public void DisplayCircleCircleAttack(Vector3 origin, Vector3 innerCirclePos, float radius1, float radius2)
    {
        if (display == null)
        {
            display = GameObject.CreatePrimitive(PrimitiveType.Plane);
            display.layer = LayerMask.NameToLayer("BehindMask");
            DestroyImmediate(display.GetComponent<Collider>());
        }
        if (display2 == null)
        {
            display2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        }
        display.transform.SetParent(transform);
        display.transform.position = origin;
        display.transform.localScale = new Vector3(radius1 / 10, 0, radius1 / 10);
        display.GetComponent<MeshRenderer>().material = circleMat;
        display2.transform.SetParent(transform);

        Vector3 dir = (innerCirclePos - transform.position).normalized;
        dir.y = 0;
        if (Vector3.Distance(innerCirclePos, transform.position) < radius1 / 2)
        {
            display2.transform.position = innerCirclePos;
        }
        else {
            display2.transform.position = (transform.position + dir * radius1 / 2);
        }
        display2.transform.localScale = new Vector3(radius2 / 10, 0, radius2 / 10);
        display2.GetComponent<MeshRenderer>().material = circleMat;
    }

    public void DisplayCircleClickAttack(Vector3 origin, float radius)
    {
        if (display == null)
        {
            display = GameObject.CreatePrimitive(PrimitiveType.Plane);
            display.layer = LayerMask.NameToLayer("BehindMask");
            DestroyImmediate(display.GetComponent<Collider>());
        }
        display.transform.SetParent(transform);
        display.transform.position = origin;
        display.transform.localScale = new Vector3(radius / 10, 0, radius / 10);
        display.GetComponent<MeshRenderer>().material = circleMat;
    }
}
