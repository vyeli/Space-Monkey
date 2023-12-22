using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Killzone");
        if (other.gameObject.tag.Equals("Player"))
        {
            EventsManager.instance.PlayerFellToKillzone();
            // EventsManager.instance.EventGameOver(false);
            GameManager.instance.RespawnPlayer();
        }
    }
}
