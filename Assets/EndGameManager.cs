using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    private void RestartGame() => SceneManager.LoadScene("SampleScene");
    private void GoToMainMenu() => SceneManager.LoadScene("MainMenu");
}
