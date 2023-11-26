using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    IGun Owner { get; }
    float Speed { get; }
    float LifeTime { get; }
    Collider collider { get; }
    // Rigidbody rb { get; }
    void Init();
    void Travel();
    void Die();
    void SetOwner(IGun owner);
}