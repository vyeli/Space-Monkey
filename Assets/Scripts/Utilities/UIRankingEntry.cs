using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRankingEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text ranking;
    [SerializeField] private TMP_Text username;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text kills;

    public void Init(int ranking, string username, int score, string time, int kills)
    {
        this.ranking.text = "#" + ranking.ToString();
        this.username.text = username;
        this.score.text = score.ToString();
        this.time.text = time;
        this.kills.text = kills.ToString();
    }
}
