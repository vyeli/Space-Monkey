using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

    void Start()
    {
        _timedScore = maxScore;
        EventsManager.instance.OnPlayerKill += OnPlayerKill;
    }

    public int Score => _timedScore + _killsScore;
    private int maxScore = 30000;
    private int _minScore = 10000;

    public int TimedScore => _timedScore;
    private int _timedScore;
    
    public int KillsScore => _killsScore;
    private int _killsScore = 0;

    void FixedUpdate()
    {
        if (!GameManager.instance.GameEnded && _timedScore > _minScore) _timedScore--;
    }

    public void OnPlayerKill(int addedScore)
    {
        _killsScore += addedScore;
    }

    public string GetScoreLetter()
    {
        if (Score >= maxScore * 0.9f) return "S";
        if (Score >= maxScore * 0.8f) return "A";
        if (Score >= maxScore * 0.7f) return "B";
        if (Score >= maxScore * 0.6f) return "C";
        return "D";
    }
}
