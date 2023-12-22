using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZoneKills : MonoBehaviour
{
    [SerializeField] GameObject miniPlatform;
    [SerializeField] GameObject previousPlatform;

    private int enemiesCount;
    private bool entered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!entered && other.CompareTag("Player") && !miniPlatform.activeSelf)
        {
            Debug.Log("Enter FinalZone");
            entered = true;
            previousPlatform.SetActive(false);

            UiManager.instance.ShowNotification("Eliminar todos los enemigos", 5f);
            enemiesCount = GameManager.instance.EnemyKills;
        }
    }

    private void Update()
    {
        int enemiesKilled = GameManager.instance.EnemyKills;
        if (enemiesKilled > enemiesCount && !miniPlatform.activeSelf)
        {
            UiManager.instance.ShowNotification(enemiesKilled - enemiesCount + "/4", 2000f);
        }
        if (enemiesKilled == enemiesCount + 4 && !miniPlatform.activeSelf)
        {
            UiManager.instance.ShowNotification("Salida desbloqueada", 2f);
            miniPlatform.SetActive(true);
        }
    }
}
