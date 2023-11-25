using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stats/EnemyStats", order = 0)]
public class EnemyStats : StaticEnemyStats
{
    [SerializeField] private EnemyStatsValues _enemyStats;

    public float Speed => _enemyStats.Speed;
    public float PatrolPointStoppingDistance => _enemyStats.PatrolPointStoppingDistance;
    public float ChaseRange => _enemyStats.ChaseRange;
}

[System.Serializable]
public struct EnemyStatsValues
{
    public float Speed;
    public float PatrolPointStoppingDistance;
    public float ChaseRange;
}