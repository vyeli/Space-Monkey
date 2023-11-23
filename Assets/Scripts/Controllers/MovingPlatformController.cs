using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    private CharacterController cc;
    private Rigidbody rb;
    private Vector3 position;

    void Update()
    {
        // Move the platform using a sine function
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time) * 5, transform.position.z);
        position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
