using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDangerzone : MonoBehaviour
{
    [SerializeField] private GameObject dangerzone;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject entryPath;
    [SerializeField] private int initialEnemyCount;
    
    private int remainingEnemies;
    private bool exitDangerzone = false;

    private void Start()
    {
        remainingEnemies = initialEnemyCount;
        EventsManager.instance.OnPlayerFellToKillzone += OnPlayerFellToKillzone;
        EventsManager.instance.OnPlayerKill += OnPlayerKill;
    }

    private void OnPlayerKill(int addedScore)
    {
        if (dangerzone.activeSelf)
        {
            remainingEnemies--;
            UiManager.instance.UpdateZoneObjectiveCounterText((initialEnemyCount - remainingEnemies) + "/" + initialEnemyCount);
        }
    }

    private void OnPlayerFellToKillzone()
    {
        if (!dangerzone.activeSelf) return; // Exit if the dangerzone is already active

        dangerzone.SetActive(false);
        obstacle.SetActive(false);
        entryPath.SetActive(true);
        Player.instance._gun.InfiniteMode = false;
        UiManager.instance.DeactivateZoneObjective();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (exitDangerzone) return; // Exit if the dangerzone has been deactivated

        if (!dangerzone.activeSelf) // Check if the dangerzone is not already active
        {
            Debug.Log("Enter Dangerzone");
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Dangerzone");
                dangerzone.SetActive(true);
                obstacle.SetActive(true);
                entryPath.SetActive(false);
                Player.instance._gun.InfiniteMode = true;

                UiManager.instance.ActivateZoneObjective();
                UiManager.instance.UpdateZoneObjectiveText("Elimina a todos los enemigos");
                UiManager.instance.UpdateZoneObjectiveCounterText((initialEnemyCount - remainingEnemies) + "/" + initialEnemyCount);
                // UiManager.instance.ShowNotification("Eliminar todos los enemigos", 3f);
            }
        }
    }

    private void Update()
    {
        if (exitDangerzone) return;

        if (dangerzone.activeSelf && remainingEnemies == 0)
        {
            dangerzone.SetActive(false);
            obstacle.SetActive(false);
            Player.instance._gun.InfiniteMode = false;

            UiManager.instance.DeactivateZoneObjective();
            UiManager.instance.ShowNotification("Ruta despejada", 2f);
            entryPath.SetActive(true);
            exitDangerzone = true;
        }
    }
}

