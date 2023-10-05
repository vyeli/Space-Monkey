using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "Stats/MovingTrapStats", order = 0)]
public class MovingTrapStats : TrapStats
{
    [SerializeField] private MovingTrapStatsValues _movingTrapStats;

    public float MovingSpeed => _movingTrapStats.MovingSpeed;
    public float RotationSpeed => _movingTrapStats.RotationSpeed;
}

[System.Serializable]
public struct MovingTrapStatsValues
{
    public float MovingSpeed;
    public float RotationSpeed;
}