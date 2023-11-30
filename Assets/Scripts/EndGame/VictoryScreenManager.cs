using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

[RequireComponent(typeof(AudioSource))]
public class VictoryScreenManager : MonoBehaviour
{
    public static VictoryScreenManager instance;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _victory;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private GameObject[] _backgroundPlanets;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private KeyCode _muteAudio = KeyCode.M;
    [SerializeField] private KeyCode _restartGame = KeyCode.R;
    [SerializeField] private KeyCode _goToMainMenu = KeyCode.Escape;

    // Score menu
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private TextMeshProUGUI _timeScoreText;
    [SerializeField] private TextMeshProUGUI _killsScoreText;
    [SerializeField] private TextMeshProUGUI _scoreLetterText;
    [SerializeField] private float _digitsChangeDelay = 1f;
    [SerializeField] private float _digitsChangeSpeed = 0.02f;
    [SerializeField] private SoundInfo _soundInfo;

    void Awake()
    {
        if (instance == null) 
        {
            instance = this; 
        } else if (instance != this) 
        {
            Destroy(gameObject); 
        }
    }

    async void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _soundInfo.MusicVolume;
        _audioSource.PlayOneShot(_victory);
        _restartButton.onClick.AddListener(RestartGame);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);

        // Setting values
        _scoreText.text = ScoreManager.instance.Score.ToString();
        _timeText.text = GameManager.instance.getTimerString();
        _killsText.text = "x" + GameManager.instance.EnemyKills.ToString();
        _timeScoreText.text = "+" + ScoreManager.instance.TimedScore.ToString();
        _killsScoreText.text = "+" + ScoreManager.instance.KillsScore.ToString();
        _scoreLetterText.text = ScoreManager.instance.GetScoreLetter();

        // Number Change effect
        StartCoroutine(digitsChangeEffect(_scoreText));
        StartCoroutine(digitsChangeEffect(_timeScoreText, 1));
        StartCoroutine(digitsChangeEffect(_killsScoreText, 1));
        StartCoroutine(letterScoreEffect(_scoreLetterText));

        await UpdateScore();
    }

    void Update()
    {
        foreach (GameObject planet in _backgroundPlanets)
        {
            planet.transform.Rotate(new Vector3(0, 0, 1), _rotationSpeed);
        }
        if (Input.GetKeyDown(_muteAudio)) ToggleMuteAudio();
        if (Input.GetKeyDown(_restartGame)) RestartGame();
        if (Input.GetKeyDown(_goToMainMenu)) GoToMainMenu();
    }

    private async Task UpdateScore()
    {
        if (DatabaseManager.instance.CurrentUser != null)
        {
            Task<RankingEntry> task = DatabaseManager.instance.GetRankingEntry("Level1");
            
            try
            {
                await task;
            }
            catch (System.Exception)
            {
                Debug.Log("Error al obtener la entrada de ranking: " + task.Exception);
                return;
            }
            
            RankingEntry entry = task.Result;
            if (entry == null || ScoreManager.instance.Score > entry.score)
            {
                Task createTask = DatabaseManager.instance.SetRankingEntry("Level1", ScoreManager.instance.Score, GameManager.instance.EnemyKills,
                ScoreManager.instance.KillsScore, GameManager.instance.getTimer(), ScoreManager.instance.TimedScore);

                await createTask;
            }
        }
    }

    private IEnumerator digitsChangeEffect(TextMeshProUGUI scoreText, int initialCharacter = 0)
    {
        char[] digitosObjetivo = scoreText.text.ToCharArray();

        for (int i = initialCharacter; i < digitosObjetivo.Length; i++)
        {
            int digitoActual = int.Parse(scoreText.text[i].ToString());
            float _timeElapsed = 0f;
            
            while (_timeElapsed < _digitsChangeDelay)
            {
                for (int j = i; j < digitosObjetivo.Length; j++)
                {
                    digitoActual = (digitoActual + 1) % 10;
                    scoreText.text = scoreText.text.Substring(0, j) + digitoActual + scoreText.text.Substring(j + 1);
                }
                yield return new WaitForSeconds(_digitsChangeSpeed);
                _timeElapsed += _digitsChangeSpeed;
            }

            // Establece el dígito objetivo
            digitoActual = int.Parse(digitosObjetivo[i].ToString());
            scoreText.text = scoreText.text.Substring(0, i) + digitoActual + scoreText.text.Substring(i + 1);
        }
    }

    private IEnumerator letterScoreEffect(TextMeshProUGUI letraText)
    {
        yield return new WaitForSeconds(4f);
        // Configura la posición del objeto de la letra
        RectTransform letraTransform = letraText.GetComponent<RectTransform>();
        Vector3 posicionInicial = letraTransform.localPosition;

        // Animación de reducción de tamaño y aumento de opacidad
        float elapsedTime = 0f;
        float tiempoEntreLetras = 1f;
        letraText.gameObject.SetActive(true);
        while (elapsedTime < tiempoEntreLetras)
        {
            letraTransform.localScale = Vector3.Lerp(new Vector3(2f, 2f, 2f), Vector3.one, elapsedTime / tiempoEntreLetras);
            Color letraColor = letraText.color;
            letraColor.a = Mathf.Lerp(0.2f, 1f, elapsedTime / tiempoEntreLetras);
            letraText.color = letraColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restablece el tamaño final y la opacidad
        letraTransform.localScale = Vector3.one;
        letraTransform.localPosition = posicionInicial;
    }

    private void ToggleMuteAudio() => _audioSource.mute = !_audioSource.mute;
    private void RestartGame()
    {
        SaveVolumes();
        GameLevelsManager.instance.LoadCurrentLevel();
    }

    private void GoToMainMenu()
    {
        SaveVolumes();
        GameLevelsManager.instance.LoadMainMenu();
    }

    private void SaveVolumes()
    {
        _soundInfo.MusicVolume = _audioSource.volume;
    }
}
