using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Object : MonoBehaviour
{
    public float speed = 15f;
    public float destroyTime = 1f;
    
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.deltaTime;
    }
}
