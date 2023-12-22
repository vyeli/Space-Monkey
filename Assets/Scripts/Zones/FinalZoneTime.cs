using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZoneTime : MonoBehaviour
{
    [SerializeField] GameObject miniPlatform;
    [SerializeField] GameObject entryPath;
    private bool isCounting = false;

    private float countDown = 15f;

    private float notificationDuration = 1f;
    private float notificationTimer = 0f;
    private bool notificationShown = false;

    private void Start()
    {
        EventsManager.instance.OnPlayerFellToKillzone += OnPlayerFellToKillzone;
    }

    private void OnPlayerFellToKillzone()
    {
        if (!isCounting) return;

        isCounting = false;
        entryPath.SetActive(true);
        countDown = 10f;
        notificationShown = false;
        UiManager.instance.DeactivateZoneObjective();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCounting && other.CompareTag("Player") && !miniPlatform.activeSelf)
        {
            Debug.Log("Enter FinalZone");
            entryPath.SetActive(false);

            UiManager.instance.ActivateZoneObjective();
            UiManager.instance.UpdateZoneObjectiveText("Sobrevivir " + countDown + " segundos");
            UiManager.instance.UpdateZoneObjectiveCounterText(countDown.ToString("F2"));

            isCounting = true;
            notificationTimer = notificationDuration;
            notificationShown = false;
        }
    }

    private void FixedUpdate()
    {
        if (isCounting && !notificationShown)
        {
            notificationTimer -= Time.fixedDeltaTime;
            if (notificationTimer <= 0)
            {
                notificationShown = true;
            }
        }

        if (isCounting && notificationShown)
        {
            countDown -= Time.fixedDeltaTime;
            // UiManager.instance.updateNotification(countDown.ToString("F2"));
            UiManager.instance.UpdateZoneObjectiveCounterText(countDown.ToString("F2"));
            if (countDown <= 0)
            {
                UiManager.instance.DeactivateZoneObjective();
                UiManager.instance.ShowNotification("Salida desbloqueada", 2f);
                miniPlatform.SetActive(true);
                isCounting = false;
            }
        }
    }
}
