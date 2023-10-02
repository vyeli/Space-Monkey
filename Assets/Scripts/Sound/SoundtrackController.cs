using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class SoundtrackController : MonoBehaviour, IListenable
{
    #region ILISTENABLE_PROPERTIES
    public AudioSource AudioSource => _audioSource;
    private AudioSource _audioSource;
    #endregion

    #region SOUNDTRACK_PROPERTIES
    public TextMeshProUGUI songNameDisplayed;
    public Slider volumeSlider;
    public Button skipButton;
    public Button pauseButton;
    private List<AudioClip> _clipsToPlay = new List<AudioClip>();
    [SerializeField] private AudioClip[] _soundtrackClips;
    #endregion

    #region ILISTENABLE_METHODS
    public void InitAudioSource()
    {
        _audioSource = GetComponent<AudioSource>();
        _clipsToPlay = GetShuffledClips(_soundtrackClips);
        AudioSource.clip = _clipsToPlay[0];
        Play();
    }

    public void PlayOneShot() => throw new System.NotImplementedException();

    public void Play() => AudioSource.Play();

    public bool IsPlaying() => AudioSource.isPlaying;
    
    public bool IsPaused() => !IsPlaying() && AudioSource.time > 0;

    public void Stop() => AudioSource.Stop();
    #endregion

    #region UNITY_EVENTS
    void Start()
    {
        InitAudioSource();
        songNameDisplayed.text = AudioSource.clip.name;
        volumeSlider.value = AudioSource.volume;
        volumeSlider.onValueChanged.AddListener(delegate { AudioSource.volume = volumeSlider.value; });
        skipButton.onClick.AddListener(delegate { ShuffleNextSong(); Play(); Debug.Log("SKIPPPPP"); });
        pauseButton.onClick.AddListener(delegate { TogglePauseState(); });
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            
            AudioSource.mute = !AudioSource.mute;
        }
        if ((!IsPlaying() && !IsPaused()) || Input.GetKeyDown(KeyCode.N))
        {
            ShuffleNextSong();
            Play();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseState();
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
        AudioSource.clip = _clipsToPlay[0];
        songNameDisplayed.text = AudioSource.clip.name;
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
            AudioSource.Pause();
        }
        else
        {
            AudioSource.UnPause();
        }
    }
    #endregion
}
