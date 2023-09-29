using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public override void Shoot()
    {
        if (_currentBulletCount > 0)
        {
            _currentBulletCount--;
            UiManager.instance.bulletCount.text = _currentBulletCount.ToString();
            GameObject bullet = Instantiate(BulletPrefab, transform.position, Rotation, BulletContainer);
            bullet.GetComponent<Bullet>().SetOwner(this);
        }
    }
}