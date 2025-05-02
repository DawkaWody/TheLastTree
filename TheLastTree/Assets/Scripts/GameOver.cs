using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gameWonUI;

    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private CanvasGroup gameWonCanvasGroup;

    [SerializeField] private float fadeDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverUI.SetActive(false);
        gameWonUI.SetActive(false);
    }

    public void TriggerGameOverUI()
    {
        gameOverUI.SetActive(true);
        StartCoroutine(Fade(0f, 1f, gameOverCanvasGroup));
        Time.timeScale = 0f;
    }

    public void TriggerGameWonUI()
    {
        gameWonUI.SetActive(true);
        StartCoroutine(Fade(0f, 1f, gameWonCanvasGroup));
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        if (SceneTransition.Instance != null)
        {
            Time.timeScale = 1f;
            if (GameManager.Instance != null)
            {
                Destroy(GameManager.Instance.gameObject);
            }
            SceneTransition.Instance.LoadSceneWithFade("SampleScene");
        }
        else
        {
            Debug.LogWarning("SceneTransitionManager not found! Loading scene normally.");
            Time.timeScale = 1f;
            if (GameManager.Instance != null)
            {
                Destroy(GameManager.Instance.gameObject);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator Fade(float start, float end, CanvasGroup group)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            group.alpha = Mathf.Lerp(start, end, elapsedTime / fadeDuration);

            elapsedTime += Time.unscaledDeltaTime; 
            yield return null;
        }

        group.alpha = end;
    }
}
