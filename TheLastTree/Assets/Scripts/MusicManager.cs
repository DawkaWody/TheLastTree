using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource _mainMusic;
    [SerializeField] private AudioSource _rainMusic;

    private bool _mainMusicPaused;
    private bool _rainMusicPaused;

    void Start()
    {        
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu")
        {
            if (_rainMusic.isPlaying) _rainMusic.Stop();
            _mainMusic.enabled = true;
            if (!_mainMusic.isPlaying) _mainMusic.Play();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator FadeOutMainMusic()
    {
        yield return FadeOutTrack(_mainMusic);
        _mainMusic.Pause();
        _mainMusicPaused = true;
    }

    public IEnumerator FadeInMainMusic()
    {
        if (_mainMusicPaused) _mainMusic.UnPause();
        else
        {
            _mainMusic.enabled = true;
            _mainMusic.Play();
        }
        _mainMusicPaused = false;
        yield return FadeInTrack(_mainMusic);
    }

    public IEnumerator FadeOutRainMusic()
    {
        yield return FadeOutTrack(_rainMusic);
        _rainMusic.Pause();
        _rainMusicPaused = true;
    }

    public IEnumerator FadeInRainMusic()
    {
        if (_rainMusicPaused) _rainMusic.UnPause();
        else
        {
            _rainMusic.enabled = true;
            _rainMusic.Play();
        }
        _rainMusicPaused = false;
        yield return FadeInTrack(_rainMusic);
    }

    private IEnumerator FadeOutTrack(AudioSource track)
    {
        float volume = 1f;
        float fadeDuration = 1.5f;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            track.volume = Mathf.Lerp(volume, 0f, elapsed / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator FadeInTrack(AudioSource track)
    {
        float volume = 1f;
        float fadeDuration = 1.5f;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            track.volume = Mathf.Lerp(0f, volume, elapsed / fadeDuration);
            yield return null;
        }
    }

    public bool isMainPlaying()
    {
        return _mainMusic.isPlaying;
    }

    public bool isRainPlaying()
    {
        return _rainMusic.isPlaying;
    }
}
