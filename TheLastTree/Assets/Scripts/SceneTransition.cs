using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [SerializeField] private CanvasGroup screenFade;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        screenFade.gameObject.SetActive(true);
        yield return StartCoroutine(Fade(0f, 1f));

        if (MusicManager.Instance.isRainPlaying())
        {
            yield return MusicManager.Instance.FadeOutRainMusic();
        }
        else if (MusicManager.Instance.isMainPlaying())
        {
            yield return MusicManager.Instance.FadeOutMainMusic();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        yield return null;

        yield return StartCoroutine(Fade(1f, 0f));
        screenFade.gameObject.SetActive(false);
        yield return StartCoroutine(MusicManager.Instance.FadeInMainMusic());
    }

    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            screenFade.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        screenFade.alpha = end;
    }
}
