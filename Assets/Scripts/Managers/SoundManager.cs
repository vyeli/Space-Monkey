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

    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;

    #region UNITY_EVENTS
    private void Start()
    {
        _musicAudioSource.clip = _backgroundMusic;
        _musicAudioSource.loop = true;

        _musicAudioSource.volume = _musicVolumeSlider.value;
        _sfxAudioSource.volume = _sfxVolumeSlider.value;

        EventsManager.instance.OnGameOver += OnGameOver;

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

    private void OnGameOver(bool isVictory)
    {
        _musicAudioSource.Stop();
        _musicAudioSource.loop = false;
        _musicAudioSource.PlayOneShot(isVictory ? _victory : _defeat);
    }
    #endregion
}
