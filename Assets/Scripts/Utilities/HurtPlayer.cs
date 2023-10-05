using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HurtPlayer : MonoBehaviour
{
    private int _damage;
    [SerializeField] private Enemy _enemy;
    void Start()
    {
        _damage = ((EnemyStats)_enemy.EntityStats).Damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) _enemy.Attack();
    }
}
