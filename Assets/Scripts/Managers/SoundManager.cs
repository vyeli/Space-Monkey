using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

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