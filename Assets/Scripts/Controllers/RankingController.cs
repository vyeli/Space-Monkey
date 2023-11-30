using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class RankingController : MonoBehaviour
{
    [Header("RankingHeader")]
    [SerializeField] private TMP_Text level;

    [Header("FirstPlace")]
    [SerializeField] private TMP_Text firstPlaceUsername;
    [SerializeField] private TMP_Text firstPlaceScore;
    [SerializeField] private TMP_Text firstPlaceTime;
    [SerializeField] private TMP_Text firstPlaceKills;

    [Header("NPlace")]
    [SerializeField] private Transform parentContainer;
    [SerializeField] private GameObject nPlacePrefab;

    [SerializeField] private int pageSize;

    async void Awake()
    {
        Task<List<RankingEntry>> task = DatabaseManager.instance.GetRanking("Level1", pageSize);

        try
        {
            await task;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
        
        List<RankingEntry> ranking = task.Result;
        firstPlaceUsername.text = ranking[0].username;
        firstPlaceScore.text = ranking[0].score.ToString();
        firstPlaceTime.text = getTimerString(ranking[0].time);
        firstPlaceKills.text = ranking[0].kills.ToString();

        for (int i=1 ; i < ranking.Count ; i++)
        {
            GameObject entry = Instantiate(nPlacePrefab, parentContainer);
            entry.GetComponent<UIRankingEntry>().Init(i+1, ranking[i].username, ranking[i].score, getTimerString(ranking[i].time), ranking[i].kills);
            entry.transform.localPosition = new Vector3(0, -(i-1) * 100, 0);
        }
    }
    
    private string getTimerString(long _levelTime)
    {
        int minutes = Mathf.FloorToInt(_levelTime / 60F);
        int seconds = Mathf.FloorToInt(_levelTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
