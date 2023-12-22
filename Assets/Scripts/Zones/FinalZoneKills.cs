using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZoneKills : MonoBehaviour
{
    [SerializeField] GameObject miniPlatform;
    [SerializeField] GameObject previousPlatform;
    [SerializeField] private int objectiveKills;

    private int initialKillsCount;
    private bool entered = false;

    private void Start()
    {
        EventsManager.instance.OnPlayerFellToKillzone += OnPlayerFellToKillzone;
    }

    private void OnPlayerFellToKillzone()
    {
        entered = false;
        Player.instance._gun.InfiniteMode = false;
        UiManager.instance.DeactivateZoneObjective();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!entered && other.CompareTag("Player") && !miniPlatform.activeSelf)
        {
            Debug.Log("Enter FinalZone");
            entered = true;
            // previousPlatform.SetActive(false);
            Player.instance._gun.InfiniteMode = true;

            UiManager.instance.ActivateZoneObjective();
            UiManager.instance.UpdateZoneObjectiveText("Elimina a " + objectiveKills + " enemigos");
            UiManager.instance.UpdateZoneObjectiveCounterText("0/" + objectiveKills);
            initialKillsCount = GameManager.instance.EnemyKills;
        }
    }

    private void Update()
    {
        if (!entered)
        {
            return;
        }
        int enemiesKilled = GameManager.instance.EnemyKills;
        if (enemiesKilled > initialKillsCount && !miniPlatform.activeSelf)
        {
            UiManager.instance.UpdateZoneObjectiveCounterText((enemiesKilled - initialKillsCount) + "/" + objectiveKills);
            // UiManager.instance.ShowNotification(enemiesKilled - enemiesCount + "/4", 2000f);
        }
        if (enemiesKilled == initialKillsCount + objectiveKills && !miniPlatform.activeSelf)
        {
            UiManager.instance.ShowNotification("Salida desbloqueada", 2f);
            Player.instance._gun.InfiniteMode = false;
            UiManager.instance.DeactivateZoneObjective();
            miniPlatform.SetActive(true);
        }
    }
}
