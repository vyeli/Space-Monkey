using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    private float progress;

    [Header("Testing graf")]
    [SerializeField] private bool fakeLoading;

    [Header("References")]
    [SerializeField] private GameObject loadingContent;

    [SerializeField] private Image loadingFillImage;

    [SerializeField] private string[] loadingFacts;
    [SerializeField] private TextMeshProUGUI loadingText;
    private void Awake()
    {
        if (fakeLoading)
        {
            loadingContent.SetActive(true);
        }
        else
        {
            loadingContent.SetActive(false);
        }
    }

    private void Start()
    {
        int shownFact = Random.Range(0, loadingFacts.Length);
        loadingText.text = loadingFacts[shownFact];
    }

    private void Update()
    {
        if (fakeLoading)
        {
            if (progress < 1f)
            {
                progress += Time.deltaTime * 0.1f;
                loadingFillImage.fillAmount = progress;
            }
        }
    }

    public void LoadScene(int _sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(_sceneIndex));
    }

    IEnumerator LoadAsynchronously(int _sceneIndex)
    {
        loadingContent.SetActive(true);
        yield return new WaitForSeconds(2f);
        AsyncOperation _operation = SceneManager.LoadSceneAsync(_sceneIndex);

        while (!_operation.isDone)
        {
            float _progress = Mathf.Clamp01(_operation.progress / 0.9f);
            float _prosentProgress = _progress * 100f;
            loadingFillImage.fillAmount = _progress;

            progress = _prosentProgress;
            yield return null;
        }
    }

    public float Progress()
    {
        return progress;
    }
}