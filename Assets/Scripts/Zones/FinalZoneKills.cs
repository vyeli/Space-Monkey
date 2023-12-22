using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZoneKills : MonoBehaviour
{
    [SerializeField] GameObject miniPlatform;
    [SerializeField] GameObject entryPath;
    [SerializeField] private int objectiveKills;

    private int remainingEnemies;
    private bool entered = false;

    private void Start()
    {
        remainingEnemies = objectiveKills;
        EventsManager.instance.OnPlayerFellToKillzone += OnPlayerFellToKillzone;
        EventsManager.instance.OnPlayerKill += OnPlayerKill;
    }

    private void OnPlayerKill(int addedScore)
    {
        if (entered)
        {
            remainingEnemies--;
            UiManager.instance.UpdateZoneObjectiveCounterText((objectiveKills - remainingEnemies) + "/" + objectiveKills);
        }
    }

    private void OnPlayerFellToKillzone()
    {
        if (!entered) return;

        entered = false;
        entryPath.SetActive(true);
        Player.instance._gun.InfiniteMode = false;
        UiManager.instance.DeactivateZoneObjective();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!entered && other.CompareTag("Player") && !miniPlatform.activeSelf)
        {
            entryPath.SetActive(false);
            Debug.Log("Enter FinalZone");
            entered = true;
            Player.instance._gun.InfiniteMode = true;

            UiManager.instance.ActivateZoneObjective();
            UiManager.instance.UpdateZoneObjectiveText("Elimina a " + objectiveKills + " enemigos");
            UiManager.instance.UpdateZoneObjectiveCounterText((objectiveKills - remainingEnemies) + "/" + objectiveKills);
        }
    }

    private void Update()
    {
        if (!entered) return;

        if (!miniPlatform.activeSelf && remainingEnemies == 0)
        {
            UiManager.instance.ShowNotification("Salida desbloqueada", 2f);
            Player.instance._gun.InfiniteMode = false;
            UiManager.instance.DeactivateZoneObjective();
            miniPlatform.SetActive(true);
        }
    }
}
