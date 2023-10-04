using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickup : Pickup
{
    public override void OnPickup(Collider player)
    {
        Gun gun = player.GetComponentInChildren<Gun>();
        gun.CurrentBulletCount += _pickupStats.PickupAmount;
        EventsManager.instance.BulletCountChange(gun.CurrentBulletCount);
    }

    public override void PickupEffect()
    {
        Debug.Log("Bullet pickup effect");
    }
}