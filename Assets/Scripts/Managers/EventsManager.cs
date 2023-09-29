using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;

    #region UNITY_EVENTS
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    #region EVENTMANAGER_ACTIONS
    public event Action<bool> OnGameOver;
    public void EventGameOver(bool isVictory)
    {
        if (OnGameOver != null) OnGameOver(isVictory);
    }
    #endregion

}