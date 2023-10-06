using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{

    [SerializeField] Transform muzzle;
    public override void Shoot()
    {
        if (_currentBulletCount == 0)
            return;
        
        GameObject bullet = Instantiate(BulletPrefab, muzzle.position, Rotation, BulletContainer);
        bullet.GetComponent<Bullet>().SetOwner(this);
            
        UpdateBulletCount();
    }
}