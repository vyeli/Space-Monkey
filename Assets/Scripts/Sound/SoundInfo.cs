using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundInfo", menuName = "Sound/SoundInfo", order = 0)]
public class SoundInfo : ScriptableObject
{
    [SerializeField] private SoundInfoValues _soundInfo;
    public float MusicVolume { get => _soundInfo.MusicVolume; set => _soundInfo.MusicVolume = value; }
    public float SFXVolume { get => _soundInfo.SFXVolume; set => _soundInfo.SFXVolume = value; }
}

[System.Serializable]
public struct SoundInfoValues
{
    public float MusicVolume;
    public float SFXVolume;
}