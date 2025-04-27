using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using Unity.Hierarchy;

public class FadeImage : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color tempColor = fadeImage.color;
        tempColor.a = 0;
        fadeImage.color = tempColor;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsed = 0f;
        Color initial = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(initial.a, 0.5f, elapsed / fadeDuration);
            Color newColor = new Color(initial.r, initial.g, initial.b, newAlpha);
            fadeImage.color = newColor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(initial.r, initial.g, initial.b, 0.5f);
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsed = 0f;
        Color initial = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(initial.a, 0, elapsed / fadeDuration);
            Color newColor = new Color(initial.r, initial.g, initial.b, newAlpha);
            fadeImage.color = newColor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(initial.r, initial.g, initial.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
