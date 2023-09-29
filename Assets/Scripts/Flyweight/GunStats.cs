using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "Stats/GunStats", order = 0)]
public class GunStats : ScriptableObject
{
    [SerializeField] private GunStatsValues _gunStats;
    
    public int Damage => _gunStats.Damage;
}

[System.Serializable]
public struct GunStatsValues
{
    public int Damage;
}