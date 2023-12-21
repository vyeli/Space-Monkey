using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDangerzone : MonoBehaviour
{
    [SerializeField] private GameObject dangerzone;
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter Dangerzone");
        if (other.gameObject.tag.Equals("Player"))
        {
            dangerzone.SetActive(true);
        }
    }
}
