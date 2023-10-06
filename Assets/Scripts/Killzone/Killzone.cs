using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Killzone");
        if (other.gameObject.tag == "Player")
        {
            // EventsManager.instance.EventGameOver(false);
            GameManager.instance.RespawnPlayer();
        }
    }
}
