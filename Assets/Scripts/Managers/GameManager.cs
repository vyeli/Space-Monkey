using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using static Enums;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance;
    public bool GameEnded => _isGameOver;
    [SerializeField] private bool _isGameOver = false;
    [SerializeField] private bool _isVictory = false;
    [SerializeField] private bool _isPaused = false;
    [SerializeField] private float _waitAfterGameOver;
    private CmdUnShoot _cmdUnShoot;

    public bool IsPaused => _isPaused;

    public Vector3 respawnPosition;

    public GameObject deathEffect;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

    void Start()
    {
        _cmdUnShoot = new CmdUnShoot();
        ChangeCursorState(false);
        EventsManager.instance.OnGameOver += OnGameOver;
        EventsManager.instance.OnGameTogglePauseState += PauseGame;
        EventsManager.instance.OnBackToMainMenuFromGame += OnBackToMainMenuFromGame;

        respawnPosition = Player.instance.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isGameOver)
        {
            _isPaused = !_isPaused;
            EventsManager.instance.GameTogglePauseState(_isPaused);
        }
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

    private IEnumerator RespawnCo()
    {
        Player.instance.gameObject.SetActive(false);

        CameraController.instance.cinemachineBrain.enabled = false;

        UiManager.instance.fadeToBlack = true;

        Instantiate(deathEffect, Player.instance.transform.position + new Vector3(0f, 1f, 0f), Player.instance.transform.rotation);

        yield return new WaitForSeconds(2f);

        UiManager.instance.fadeFromBlack = true;

        
        Player.instance.transform.position = respawnPosition;
        CameraController.instance.cinemachineBrain.enabled = true;
        Player.instance.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
    }

    #region ACTIONS
    private void OnGameOver(bool isVictory)
    {
        _isGameOver = true;
        _isVictory = isVictory;
        ChangeCursorState(true);
        StartCoroutine(LoadCreditsScreenCo());
    }

    IEnumerator LoadCreditsScreenCo()
    {
        yield return new WaitForSeconds(_waitAfterGameOver);
        GameLevelsManager.instance.LoadEndGame();
    }

    private void OnBackToMainMenuFromGame()
    {
        ChangeCursorState(true);
        ChangePauseState(false);
        GameLevelsManager.instance.LoadMainMenu();
    }

    public bool PlayerWon()
    {
        return _isVictory;
    }

    private void ChangePauseState(bool pause)
    {
        if (!pause) EventQueueManager.instance.AddCommand(_cmdUnShoot);
        _isPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }

    private void ChangeCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void PauseGame(bool pause)
    {
        ChangePauseState(pause);
        ChangeCursorState(pause);
    }
    #endregion

}
