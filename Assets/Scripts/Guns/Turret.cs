using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Gun
{
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpeed;

    public new int CurrentBulletCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        bullet.GetComponent<Bullet>().SetOwner(this);
    }
}