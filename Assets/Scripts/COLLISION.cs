using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COLLISION : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision !");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger !");
    }

    void Update()
    {
       
    }
}