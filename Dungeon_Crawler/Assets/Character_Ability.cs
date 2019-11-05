using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Ability : NetworkBehaviour
{
    public Ability_Display display;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocal)
        {
            Vector3 dir = (mouseWorldPosition() - transform.position);
            if (Input.GetKey(KeyCode.Alpha1))
            {
                display.DisplayRangedAttack(transform.position, dir.normalized, Mathf.Min(dir.magnitude, 10f), 2f);
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                display.DisplayCircleClickAttack(transform.position, 20f);
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                display.DisplayCircleCircleAttack(transform.position, mouseWorldPosition(), 20f, 5f);
            }
            else
            {
                display.ResetDisplay();
            }
        }
    }

    Vector3 mouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.point;
        return Vector3.zero;
    }
}
