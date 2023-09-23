using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuAudioController : MonoBehaviour
{

    public AudioSource audioSource;
    public TextMeshProUGUI songNameDisplayed;
    public AudioClip[] audioClips;
    public Slider volumeSlider;
    public Button skipButton;
    private List<AudioClip> _clipsToPlay = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        _clipsToPlay = getClipsShuffled(audioClips);
        audioSource.clip = _clipsToPlay[0];
        songNameDisplayed.text = audioSource.clip.name;
        audioSource.Play();
        volumeSlider.value = audioSource.volume;
        volumeSlider.onValueChanged.AddListener(delegate { audioSource.volume = volumeSlider.value; });
        skipButton.onClick.AddListener(delegate { playNextSong(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            playNextSong();
        }
    }

    private void playNextSong() {
        _clipsToPlay.RemoveAt(0);
        if (_clipsToPlay.Count == 0)
        {
            _clipsToPlay = getClipsShuffled(audioClips);
        }
        audioSource.clip = _clipsToPlay[0];
        songNameDisplayed.text = audioSource.clip.name;
        audioSource.Play();
    }

    private List<AudioClip> getClipsShuffled(AudioClip[] audioClips) {
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
}
