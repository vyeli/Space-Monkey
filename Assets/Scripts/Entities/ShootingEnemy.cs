using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy, IGun
{

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpeed;
    protected override void AttackAction()
    {
        base.AttackAction();
        Shoot();
        // bullet.GetComponent<Rigidbody>().AddForce(_bulletSpawn.forward * _bulletSpeed, ForceMode.Impulse);
    }

    public GameObject BulletPrefab => _bulletPrefab;

    public int Damage => _enemyStats.Damage;

    public int CurrentBulletCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        bullet.GetComponent<Bullet>().SetOwner(this);
    }
}
