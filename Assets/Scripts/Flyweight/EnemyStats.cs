using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stats/EnemyStats", order = 0)]
public class EnemyStats : EntityStats
{
    [SerializeField] private EnemyStatsValues _enemyStats;

    public float Speed => _enemyStats.Speed;
    public float IdleWaitTime => _enemyStats.IdleWaitTime;
    public float PatrolPointStoppingDistance => _enemyStats.PatrolPointStoppingDistance;
    public float ChaseRange => _enemyStats.ChaseRange;
    public float AttackRange => _enemyStats.AttackRange;
    public float AttackCooldownTime => _enemyStats.AttackCooldownTime;
    public float DespawnCooldownTime => _enemyStats.DespawnCooldownTime;
    public int Damage => _enemyStats.Damage;
}

[System.Serializable]
public struct EnemyStatsValues
{
    public float Speed;
    public float IdleWaitTime;
    public float PatrolPointStoppingDistance;
    public float ChaseRange;
    public float AttackRange;
    public float AttackCooldownTime;
    public float DespawnCooldownTime;
    public int Damage;
}