using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZoneTime : MonoBehaviour
{
    [SerializeField] GameObject miniPlatform;
    [SerializeField] GameObject previousPlatform;
    private bool isCounting = false;

    private float countDown = 10f;

    private float notificationDuration = 1.5f;
    private float notificationTimer = 0f;
    private bool notificationShown = false;

    private void Start()
    {
        EventsManager.instance.OnPlayerFellToKillzone += OnPlayerFellToKillzone;
    }

    private void OnPlayerFellToKillzone()
    {
        isCounting = false;
        countDown = 10f;
        notificationShown = false;
        UiManager.instance.DeactivateZoneObjective();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCounting && other.CompareTag("Player") && !miniPlatform.activeSelf)
        {
            Debug.Log("Enter FinalZone");

            UiManager.instance.ActivateZoneObjective();
            UiManager.instance.UpdateZoneObjectiveText("Sobrevivir 10 segundos");
            UiManager.instance.UpdateZoneObjectiveCounterText("10.00");

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
