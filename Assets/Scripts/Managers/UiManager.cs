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
        EventsManager.instance.OnCharacterLifeChange += OnCharacterLifeChange;
        EventsManager.instance.OnBulletCountChange += OnBulletCountChange;
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

    #region HEALTH_UI_LOGIC
    [SerializeField] private TextMeshProUGUI _characterLifeText;
    private void OnCharacterLifeChange(int life)
    {
        _characterLifeText.text = life.ToString();
    }
    #endregion

    #region BULLETS_UI_LOGIC
    [SerializeField] private TextMeshProUGUI _bulletCountText;

    private void OnBulletCountChange(int bulletCount)
    {
        _bulletCountText.text = bulletCount.ToString();
        // Acá agrego el sonido, sfx de disparar (si hay)
    }
    #endregion
}
