using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickupStats", menuName = "Stats/PickupStats", order = 0)]
public class PickupStats : ScriptableObject
{
    [SerializeField] private PickupStatsValues _pickupStats;
    public int PickupAmount => _pickupStats.PickupAmount;
    public float RotationSpeed => _pickupStats.RotationSpeed;
    public float FloatingSpeed => _pickupStats.FloatingSpeed;
    public float FloatingHeight => _pickupStats.FloatingHeight;
}

[System.Serializable]
public struct PickupStatsValues
{
    public int PickupAmount;
    public float RotationSpeed;
    public float FloatingSpeed;
    public float FloatingHeight;
}