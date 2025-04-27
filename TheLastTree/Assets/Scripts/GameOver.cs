using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gameWonUI;

    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverUI.SetActive(false);
    }

    public void TriggerGameOverUI()
    {
        gameOverUI.SetActive(true);
        StartCoroutine(FadeGameOver(0f, 1f));
        Time.timeScale = 0f;
    }

    public void TriggerGameWonUI()
    {
        gameWonUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        StartCoroutine(FadeGameOver(1f, 0f));
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator FadeGameOver(float start, float end)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(start, end, elapsedTime / fadeDuration);
            gameOverCanvasGroup.alpha = alpha;

            elapsedTime += Time.unscaledDeltaTime; 
            yield return null;
        }

        gameOverCanvasGroup.alpha = end;

        if(start == 1f)
        {
            gameOverUI.SetActive(false);
        }
        else
        {
            gameOverUI.SetActive(true);
        }

    }
}
