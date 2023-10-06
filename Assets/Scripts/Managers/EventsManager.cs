using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;

    #region UNITY_EVENTS
    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }
    #endregion

    #region EVENTMANAGER_ACTIONS
    public event Action<bool> OnGameOver;
    public void EventGameOver(bool isVictory)
    {
        if (OnGameOver != null) OnGameOver(isVictory);
    }

    public event Action OnBackToMainMenuFromGame;
    public void EventBackToMainMenuFromGame()
    {
        if (OnBackToMainMenuFromGame != null) OnBackToMainMenuFromGame();
    }

    public event Action<int> OnCharacterLifeChange;

    public void CharacterLifeChange(int life)
    {
        if (OnCharacterLifeChange != null) OnCharacterLifeChange(life);
    }

    public event Action OnPlayerDamaged;

    public void PlayerDamaged()
    {
        if (OnPlayerDamaged != null) OnPlayerDamaged();
    }

    public event Action<int> OnBulletCountChange;

    public void BulletCountChange(int bulletCount)
    {
        if (OnBulletCountChange != null) OnBulletCountChange(bulletCount);
    }

    public event Action<bool> OnGameTogglePauseState;

    public void GameTogglePauseState(bool pause)
    {
        if (OnGameTogglePauseState != null) OnGameTogglePauseState(pause);
    }
    #endregion

}