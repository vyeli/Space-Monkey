using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount;
    public bool isFullHeal;

    public GameObject healthEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            Destroy(gameObject);

            Instantiate(healthEffect, transform.position, transform.rotation);

            if (isFullHeal)
            {
                HealthManager.instance.ResetHealth();
            } 
            else {
               HealthManager.instance.AddHealth(healAmount);
            }
        }
    }

}
