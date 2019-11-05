using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public bool locked = false;
    public Transform target;
    public float ScrollSpeed = 15f;

    Vector3 offset;

    private void Start()
    {
        NetworkManager.instance.onSetup += OnSetup;
    }

    public void OnSetup(GameObject player)
    {
        target = player.transform;
        offset = transform.position - target.position;
        transform.LookAt(target);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            locked = !locked;
        }
        if (locked)
        {
            transform.LookAt(target);
            transform.position = target.position + offset;
        }
        else
        {
            if (Input.mousePosition.y >= Screen.height * 0.95)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
            }
            if (Input.mousePosition.y <= Screen.height * 0.05)
            {
                transform.Translate(-Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
            }
            if (Input.mousePosition.x >= Screen.width * 0.95)
            {
                transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
            }
            if (Input.mousePosition.x <= Screen.width * 0.05)
            {
                transform.Translate(-Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
            }
            Camera.main.transform.position += Vector3.up * Input.GetAxis("Mouse ScrollWheel");
        }
    }
}
