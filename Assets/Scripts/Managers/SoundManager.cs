using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }

    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _victory;
    [SerializeField] private AudioClip _defeat;
    [SerializeField] private AudioClip _playerDamage;

    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private SoundInfo _soundInfo;

    #region UNITY_EVENTS
    private void Start()
    {
        _musicAudioSource.clip = _backgroundMusic;
        _musicAudioSource.loop = true;

        _musicVolumeSlider.value = _soundInfo.MusicVolume;
        _sfxVolumeSlider.value = _soundInfo.SFXVolume;

        // _musicAudioSource.volume = _musicVolumeSlider.value;
        // _sfxAudioSource.volume = _sfxVolumeSlider.value;

        EventsManager.instance.OnBackToMainMenuFromGame += OnBackToMainMenuFromGame;
        EventsManager.instance.OnGameOver += OnGameOver;
        EventsManager.instance.OnPlayerDamaged += OnPlayerDamaged;

        _musicAudioSource.Play();
    }
    #endregion

    #region EVENTS
    public void MusicVolumeChange()
    {
        _musicAudioSource.volume = _musicVolumeSlider.value;
    }

    public void PlaySFX(AudioClip clip)
    {
        _sfxAudioSource.PlayOneShot(clip);
    }

    public void SFXVolumeChange()
    {
        _sfxAudioSource.volume = _sfxVolumeSlider.value;
    }

    private void OnBackToMainMenuFromGame() => SaveVolumes();

    private void OnPlayerDamaged() => _sfxAudioSource.PlayOneShot(_playerDamage);

    private void OnGameOver(bool isVictory)
    {
        _musicAudioSource.Stop();
        _musicAudioSource.loop = false;
        _sfxAudioSource.PlayOneShot(isVictory ? _victory : _defeat);
        SaveVolumes();
    }

    private void SaveVolumes()
    {
        _soundInfo.MusicVolume = _musicVolumeSlider.value;
        _soundInfo.SFXVolume = _sfxVolumeSlider.value;
    }
    #endregion
}
