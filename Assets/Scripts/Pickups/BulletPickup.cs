using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickup : Pickup
{
    public override void OnPickup(Collider player)
    {
        Gun gun = player.GetComponentInChildren<Gun>();
        gun.CurrentBulletCount += _pickupStats.PickupAmount;
        if (!gun.InfiniteMode)
            EventsManager.instance.BulletCountChange(gun.CurrentBulletCount.ToString());
    }

    public override void PickupEffect()
    {
        Debug.Log("Bullet pickup effect");
    }
}