using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject gameplayUI;

    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    void Start()
    {
        mainMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        Time.timeScale = 0f;
    }
    public void Play()
    {
        Debug.Log("Pressed");
        StartCoroutine(FadeOutMainMenu());
    }

    private IEnumerator FadeOutMainMenu()
    {
        Debug.Log("FadeOutMainMenu started");
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            mainMenuCanvasGroup.alpha = alpha;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        Debug.Log("here");
        mainMenuCanvasGroup.alpha = 0f;

        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        Time.timeScale = 1f;
    }


    public void Quit()
    {
        Application.Quit(); 
    }
}
