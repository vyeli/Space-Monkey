using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _victory;
    [SerializeField] private AudioClip _defeat;
    [SerializeField] private Slider _soundVolumeSlider;
    private AudioSource _audioSource;

    #region UNITY_EVENTS
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        EventsManager.instance.OnGameOver += OnGameOver;
        _audioSource.clip = _backgroundMusic;
        _audioSource.loop = true;
        _soundVolumeSlider.value = _audioSource.volume;
        _audioSource.Play();
    }
    #endregion

    #region EVENTS
    public void SoundVolumeChange() => _audioSource.volume = _soundVolumeSlider.value;
    private void OnGameOver(bool isVictory)
    {
        _audioSource.Stop();
        _audioSource.loop = false;
    }
    #endregion
}