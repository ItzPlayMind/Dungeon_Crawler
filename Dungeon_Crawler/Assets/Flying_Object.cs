using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Object : MonoBehaviour
{
    /*private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }*/

    private void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 5;
    }
}
