using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAYS : MonoBehaviour
{
    void Update()
    {
        Vector3 look = transform.TransformDirection(Vector3.forward);

        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);

        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);

        foreach(RaycastHit hit in hits)
        {
            Debug.Log($"Raycast {hit.collider.gameObject.name}!");
        }
    }
}
