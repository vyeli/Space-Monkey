using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }

    private void Start()
    {
        EventsManager.instance.OnCharacterLifeChange += OnCharacterLifeChange;
        EventsManager.instance.OnBulletCountChange += OnBulletCountChange;
        EventsManager.instance.OnGameTogglePauseState += OnTogglePauseState;
    }

    public Image blackScreen;
    public float fadeSpeed = 1f;
    public bool fadeToBlack, fadeFromBlack;

    // Update is called once per frame
    void Update()
    {
        if (fadeToBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 
                               Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (fadeFromBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b,
                               Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }
    }

    [SerializeField] private TextMeshProUGUI _scoreText;

    public void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    [SerializeField] private TextMeshProUGUI _timerText;

    public void UpdateTimer(string time)
    {
        _timerText.text = time;
    }

    #region HEALTH_UI_LOGIC
    [SerializeField] private TextMeshProUGUI _characterLifeText;
    private void OnCharacterLifeChange(int life)
    {
        UpdateCharacterLife(life);
    }
    public void UpdateCharacterLife(int life)
    {
        _characterLifeText.text = life.ToString();
    }
    #endregion

    #region BULLETS_UI_LOGIC
    [SerializeField] private TextMeshProUGUI _bulletCountText;

    private void OnBulletCountChange(int bulletCount)
    {
        UpdateBulletCount(bulletCount);
    }

    public void UpdateBulletCount(int bulletCount)
    {
        _bulletCountText.text = bulletCount.ToString();
    }
    #endregion

    #region PAUSE_UI_LOGIC
    [SerializeField] private GameObject _pauseMenu;
    private void OnTogglePauseState(bool pause)
    {
        _pauseMenu.SetActive(pause);
        if (_optionsMenu.activeSelf) _optionsMenu.SetActive(false);
    }
    [SerializeField] private GameObject _optionsMenu;
    private void ToggleOptionsMenu(bool options)
    {
        _pauseMenu.SetActive(!options);
        _optionsMenu.SetActive(options);
    }
    public void OpenOptionsMenu() => ToggleOptionsMenu(true);
    public void CloseOptionsMenu() => ToggleOptionsMenu(false);
    #endregion
}
