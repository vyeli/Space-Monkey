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
        _killsText.text = "0";
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

    [SerializeField] private TextMeshProUGUI _killsText;

    public void UpdateKillsCount(int kills)
    {
        _killsText.text = kills.ToString();
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

    #region NOTIFICATIONS_UI_LOGIC
    [SerializeField] private GameObject _notificationPanel;
    [SerializeField] private TextMeshProUGUI _notificationText;
    [SerializeField] private GameObject _zoneObjectivePanel;
    [SerializeField] private TextMeshProUGUI _zoneObjectiveText;
    [SerializeField] private TextMeshProUGUI _zoneObjectiveCounterText;

    public void ActivateZoneObjective() => _zoneObjectivePanel.SetActive(true);

    public void DeactivateZoneObjective() => _zoneObjectivePanel.SetActive(false);

    public void UpdateZoneObjectiveText(string objective)
    {
        _zoneObjectiveText.text = objective;
    }

    public void UpdateZoneObjectiveCounterText(string counterText)
    {
        _zoneObjectiveCounterText.text = counterText;
    }

    public void setNotification(string notification)
    {
        _notificationText.text = notification;
        _notificationPanel.SetActive(true);
    }

    public void HideNotification()
    {
        _notificationPanel.SetActive(false);
    }

    public void ShowNotification(string notification, float duration)
    {
        StartCoroutine(ShowNotificationCoroutine(notification, duration));
    }

    private IEnumerator ShowNotificationCoroutine(string notification, float duration)
    {   
        yield return new WaitForSeconds(0.25f);
        setNotification(notification);
        yield return new WaitForSeconds(duration);
        HideNotification();
    }

    public void updateNotification(string notification)
    {
        _notificationText.text = notification;
    }

    #endregion
}
