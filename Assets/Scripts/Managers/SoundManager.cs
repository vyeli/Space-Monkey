using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _victory;
    [SerializeField] private AudioClip _defeat;
    private AudioSource _audioSource;

    #region UNITY_EVENTS
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        EventsManager.instance.OnGameOver += OnGameOver;
    }
    #endregion

    #region EVENTS
    private void OnGameOver(bool isVictory)
    {
        // Play win audio (if needed)
    }
    #endregion
}