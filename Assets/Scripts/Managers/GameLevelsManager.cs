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

    public void LoadDefeatScreen() => SceneManager.LoadScene((int)Levels.DefeatScreen);

    public void LoadVictoryScreen() => SceneManager.LoadScene((int)Levels.VictoryScreen);

    public void LoadRankingScreen() => SceneManager.LoadScene((int)Levels.Ranking);

    public void ExitGame() => Application.Quit();
}