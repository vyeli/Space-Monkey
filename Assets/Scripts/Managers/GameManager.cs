using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance;

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        respawnPosition = PlayerController.instance.transform.position;
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
        PlayerController.instance.gameObject.SetActive(false);

        CameraController.instance.cinemachineBrain.enabled = false;

        UiManager.instance.fadeToBlack = true;

        Instantiate(deathEffect, PlayerController.instance.transform.position + new Vector3(0f, 1f, 0f), PlayerController.instance.transform.rotation);

        yield return new WaitForSeconds(2f);

        UiManager.instance.fadeFromBlack = true;

        
        PlayerController.instance.transform.position = respawnPosition;
        CameraController.instance.cinemachineBrain.enabled = true;
        PlayerController.instance.gameObject.SetActive(true);

        HealthManager.instance.ResetHealth();
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
    }

}
