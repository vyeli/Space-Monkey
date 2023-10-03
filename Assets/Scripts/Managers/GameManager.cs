using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance;
    [SerializeField] private bool _isGameOver = false;
    [SerializeField] private bool _isVictory = false;

    public Vector3 respawnPosition;

    public GameObject deathEffect;

    private void Awake()
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
        EventsManager.instance.OnGameOver += OnGameOver;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        respawnPosition = Player.instance.transform.position;
    }

    void Update()
    {
        
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

        HealthManager.instance.ResetHealth();
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
        LoadCreditsScreen();
    }

    public bool PlayerWon()
    {
        return _isVictory;
    }

    private void LoadCreditsScreen()
    {
        SceneManager.LoadScene("EndGame");
    }
    #endregion

}
