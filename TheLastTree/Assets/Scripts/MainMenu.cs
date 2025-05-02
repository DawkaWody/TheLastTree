using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;

    [SerializeField] private CanvasGroup screenFade;

    private void Awake()
    {
        DontDestroyOnLoad(screenFade.gameObject);
    }
    void Start()
    {
        mainMenuUI.SetActive(true);
    }
    public void Play()
    {
        SceneTransition.Instance.LoadSceneWithFade("SampleScene");
    }

    public void Quit()
    {
        Application.Quit(); 
    }
}
