using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "Stats/TrapStats", order = 0)]
public class TrapStats : ScriptableObject
{
    [SerializeField] private TrapStatsValues _trapStats;

    public int Damage => _trapStats.Damage;
}

[System.Serializable]
public struct TrapStatsValues
{
    public int Damage;
}