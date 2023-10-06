using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;
using UnityEngine.SceneManagement;

public class GameLevelsManager : MonoBehaviour
{
    public static GameLevelsManager instance;
    private Levels currentLevel = Levels.Level1;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

    public void LoadCurrentLevel() => SceneManager.LoadScene((int)currentLevel);

    public void LoadMainMenu() => SceneManager.LoadScene((int)Levels.MainMenu);

    public void LoadEndGame() => SceneManager.LoadScene((int)Levels.EndGame);

    public void ExitGame() => Application.Quit();
}