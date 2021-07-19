using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAY : MonoBehaviour
{  
    void Update()
    {
        Vector3 look = transform.TransformDirection(Vector3.forward);

        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, look, out hit, 10))
        {
            Debug.Log($"Raycast {hit.collider.gameObject.name}!");
        }
    }
}
