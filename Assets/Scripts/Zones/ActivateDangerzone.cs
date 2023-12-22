using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDangerzone : MonoBehaviour
{
    [SerializeField] private GameObject dangerzone;
    [SerializeField] private GameObject obstacle;

    private int enemiesCount;
    private bool exitDangerzone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (exitDangerzone) return; // Exit if the dangerzone has been deactivated

        if (!dangerzone.activeSelf) // Check if the dangerzone is not already active
        {
            Debug.Log("Enter Dangerzone");
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Dangerzone");
                enemiesCount = GameManager.instance.EnemyKills;
                dangerzone.SetActive(true);
                obstacle.SetActive(true);
                Player.instance._gun.InfiniteMode = true;

                UiManager.instance.ActivateZoneObjective();
                UiManager.instance.UpdateZoneObjectiveText("Elimina a todos los enemigos");
                UiManager.instance.UpdateZoneObjectiveCounterText("0/3");
                // UiManager.instance.ShowNotification("Eliminar todos los enemigos", 3f);
            }
        }
    }

    private void Update()
    {
        if (exitDangerzone) return;

        if (dangerzone.activeSelf)
        {
            UiManager.instance.UpdateZoneObjectiveCounterText((GameManager.instance.EnemyKills - enemiesCount) + "/3");
            if (GameManager.instance.EnemyKills == enemiesCount + 3)
            {
                dangerzone.SetActive(false);
                obstacle.SetActive(false);
                Player.instance._gun.InfiniteMode = false;

                UiManager.instance.DeactivateZoneObjective();
                UiManager.instance.ShowNotification("Ruta despejada", 2f);
                exitDangerzone = true;
            }
        }
    }
}

