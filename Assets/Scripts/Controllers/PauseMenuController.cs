using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public void ResumeGame() => EventsManager.instance.GameTogglePauseState(false);
    public void OpenOptionsMenu() => UiManager.instance.OpenOptionsMenu();
    public void ChangeSoundVolume() => SoundManager.instance.MusicVolumeChange();
    public void ChangeSFXVolume() => SoundManager.instance.SFXVolumeChange();
    public void CloseOptionsMenu() => UiManager.instance.CloseOptionsMenu();
    public void ReturnToMainMenu() => EventsManager.instance.EventBackToMainMenuFromGame();
}