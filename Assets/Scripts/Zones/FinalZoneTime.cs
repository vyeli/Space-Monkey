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

    private float notificationDuration = 2f;
    private float notificationTimer = 0f;
    private bool notificationShown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCounting && other.CompareTag("Player") && !miniPlatform.activeSelf)
        {
            Debug.Log("Enter FinalZone");
            previousPlatform.SetActive(false);

            UiManager.instance.ShowNotification("Sobrevivir 10 segundos", 100f);
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
            UiManager.instance.updateNotification(countDown.ToString("F2"));
            if (countDown <= 0)
            {
                UiManager.instance.ShowNotification("Salida desbloqueada", 2f);
                miniPlatform.SetActive(true);
                isCounting = false;
            }
        }
    }
}
