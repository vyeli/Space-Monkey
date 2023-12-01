using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundtrackPlayer : MonoBehaviour, IListenable
{

    #region SOUNDTRACK_PROPERTIES
    [SerializeField] private TextMeshProUGUI songNameDisplayed;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private AudioClip[] _soundtrackClips;
    [SerializeField] private Image _pauseImage;
    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private Slider _optionsVolumeSlider;
    [SerializeField] private Slider _optionsSFXSlider;
    [SerializeField] private SoundInfo _soundInfo;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private List<AudioClip> _clipsToPlay = new List<AudioClip>();
    #endregion

    #region ILISTENABLE_METHODS
    public void InitAudioSource()
    {

        _clipsToPlay = GetShuffledClips(_soundtrackClips);
        musicSource.clip = _clipsToPlay[0];
        Play();
    }

    public void PlayOneShot() => throw new System.NotImplementedException();

    public void Play() => musicSource.Play();

    public void PlaySFX(AudioClip clip) => sfxSource.PlayOneShot(clip);

    public bool IsPlaying() => musicSource.isPlaying;
    
    public bool IsPaused() => !IsPlaying() && musicSource.time > 0;

    public void Stop() => musicSource.Stop();
    #endregion

    #region UNITY_EVENTS
    void Start()
    {
        InitAudioSource();

        songNameDisplayed.text = musicSource.clip.name;

        volumeSlider.value = _soundInfo.MusicVolume;
        _optionsVolumeSlider.value = _soundInfo.MusicVolume;
        _optionsSFXSlider.value = _soundInfo.SFXVolume;

        volumeSlider.onValueChanged.AddListener(delegate { musicSource.volume = volumeSlider.value; _optionsVolumeSlider.value = volumeSlider.value; });
        _optionsVolumeSlider.onValueChanged.AddListener(delegate { musicSource.volume = _optionsVolumeSlider.value; volumeSlider.value = _optionsVolumeSlider.value; });

        _optionsSFXSlider.onValueChanged.AddListener(delegate { sfxSource.volume = _optionsSFXSlider.value; });

        skipButton.onClick.AddListener(delegate { ShuffleNextSong(); Play(); });
        pauseButton.onClick.AddListener(delegate { TogglePauseState(); });
    }

    void Update()
    {
        if ((!IsPlaying() && !IsPaused()))
        {
            ShuffleNextSong();
            Play();
        }
    }
    #endregion

    #region SOUNDTRACK_METHODS

    private void ShuffleNextSong() {
        _clipsToPlay.RemoveAt(0);
        if (_clipsToPlay.Count == 0)
        {
            _clipsToPlay = GetShuffledClips(_soundtrackClips);
        }
        musicSource.clip = _clipsToPlay[0];
        songNameDisplayed.text = musicSource.clip.name;
    }

    private List<AudioClip> GetShuffledClips(AudioClip[] audioClips) {
        List<AudioClip> shuffledClips = new List<AudioClip>(audioClips);
        
        int n = audioClips.Length;
        System.Random rng = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            AudioClip clip = shuffledClips[k];
            shuffledClips[k] = shuffledClips[n];
            shuffledClips[n] = clip;
        }

        return shuffledClips;
    }

    private void TogglePauseState()
    {
        if (!IsPaused())
        {
            musicSource.Pause();
            _pauseImage.sprite = _playSprite;
        }
        else
        {
            musicSource.UnPause();
            _pauseImage.sprite = _pauseSprite;
        }
    }

    public void SaveVolumes()
    {
        _soundInfo.MusicVolume = musicSource.volume;
        _soundInfo.SFXVolume = sfxSource.volume;
    }
    #endregion
}
