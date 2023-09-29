using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun  {
    GameObject BulletPrefab { get; }
    int Damage { get; }
    int CurrentBulletCount { get; }
    void Shoot();
}