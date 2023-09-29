using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "Stats/PlayerStats", order = 0)]
public class PlayerStats : EntityStats
{
    [SerializeField] private PlayerStatsValues _playerStats;

    public float MovementSpeed => _playerStats.MovementSpeed;
}

[System.Serializable]
public struct PlayerStatsValues
{
    public float MovementSpeed;
}
