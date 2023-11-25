using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticEnemyStats", menuName = "Stats/StaticEnemyStats", order = 0)]
public class StaticEnemyStats : EntityStats
{
    [SerializeField] private StaticEnemyStatsValues _staticEnemyStats;
    public float IdleWaitTime => _staticEnemyStats.IdleWaitTime;
    public float AttackRange => _staticEnemyStats.AttackRange;
    public float AttackCooldownTime => _staticEnemyStats.AttackCooldownTime;
    public float DespawnCooldownTime => _staticEnemyStats.DespawnCooldownTime;
    public int Damage => _staticEnemyStats.Damage;
    public int Score => _staticEnemyStats.Score;
}

[System.Serializable]
public struct StaticEnemyStatsValues
{
    public float IdleWaitTime;
    public float AttackRange;
    public float AttackCooldownTime;
    public float DespawnCooldownTime;
    public int Damage;
    public int Score;
}