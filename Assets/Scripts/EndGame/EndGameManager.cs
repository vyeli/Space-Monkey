using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static Enums;

[RequireComponent(typeof(AudioSource))]
public class EndGameManager : MonoBehaviour
{
    public static EndGameManager instance;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _victory;
    [SerializeField] private AudioClip _defeat;
    [SerializeField] private TextMeshProUGUI _gameOverMessage;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private GameObject[] _backgroundPlanets;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private KeyCode _muteAudio = KeyCode.M;
    [SerializeField] private KeyCode _restartGame = KeyCode.R;
    [SerializeField] private KeyCode _goToMainMenu = KeyCode.Escape;

    void Awake()
    {
        if (instance == null) 
        {
            instance = this; 
        } else if (instance != this) 
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (GameManager.instance.PlayerWon())
        {
            _audioSource.PlayOneShot(_victory);
            _gameOverMessage.text = "¡Ganaste!";

        }
        else
        {
            _audioSource.PlayOneShot(_defeat);
            _gameOverMessage.text = "¡Perdiste!";
        }
        _restartButton.onClick.AddListener(RestartGame);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void Update()
    {
        foreach (GameObject planet in _backgroundPlanets)
        {
            planet.transform.Rotate(new Vector3(0, 0, 1), _rotationSpeed);
        }
        if (Input.GetKeyDown(_muteAudio)) ToggleMuteAudio();
        if (Input.GetKeyDown(_restartGame)) RestartGame();
        if (Input.GetKeyDown(_goToMainMenu)) GoToMainMenu();
    }

    private void ToggleMuteAudio() => _audioSource.mute = !_audioSource.mute;
    private void RestartGame() => GameLevelsManager.instance.LoadCurrentLevel();
    private void GoToMainMenu() => GameLevelsManager.instance.LoadMainMenu();
}
