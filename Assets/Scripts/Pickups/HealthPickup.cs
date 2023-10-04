using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public override void OnPickup(Collider player)
    {
        Actor _player = player.GetComponent<Actor>();
        _player.Life += _pickupStats.PickupAmount;
        EventsManager.instance.CharacterLifeChange(_player.Life);
    }

    public override void PickupEffect()
    {
        Debug.Log("Health pickup effect");
    }
}

/*
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
*/
