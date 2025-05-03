using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public static GameManager Instance;
    private PlayerHealth _playerHealth;
    private MainTree _mainTree;

    [SerializeField] private int afterSignal;

    [SerializeField] private int _timeBeforeMin;
    [SerializeField] private int _timeBeforeMax;
    [SerializeField] private int _durationMin;
    [SerializeField] private int _durationMax;

    [SerializeField] private float damageInterval;
    [SerializeField] private int _rainDamage;

    [SerializeField] private Image darkenImage;
    [SerializeField] private float targetAlpha;

    [SerializeField] private TextMeshProUGUI warningText;

    [SerializeField] private ParticleSystem rainParticles;

    //[SerializeField] private AudioSource _mainMusic;
    //[SerializeField] private AudioSource _rainMusic;

    private GameObject rainEffect;

    private bool isGameRunning = false;
    //private bool _mainMusicPaused;
    //private bool _rainMusicPaused;

    private GameOver _gameOver;

    [SerializeField] private GameObject gameOverUI;

    void Awake()
    {
        Debug.Log("GameManager Awake");
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameOver = FindAnyObjectByType<GameOver>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerHealth = player.GetComponent<PlayerHealth>();
        }

        //_mainMusicPaused = false;
        //_rainMusicPaused = false;

        StartGame(player.transform);
    }

    private void StartGame(Transform player)
    {
        if (MapGenerator.Instance != null)
        {
            MapGenerator.Instance.GenerateMap();
        }

        GameObject tree = GameObject.FindWithTag("MainTree");
        if (tree != null)
        {
            _mainTree = tree.GetComponent<MainTree>();
        }

        player.position = MapGenerator.Instance.GetPlayerSpawnpoint().position;
        isGameRunning = true;
        StartCoroutine(RainCycle());
    }

    private IEnumerator RainCycle()
    {
        while (isGameRunning)
        {
            int timeBefore = Random.Range(_timeBeforeMin, _timeBeforeMax);
            int duration = Random.Range(_durationMin, _durationMax);

            yield return new WaitForSeconds(timeBefore - 5);

            // signal for storm
            Color color = darkenImage.color;

            Coroutine darkenBefore = StartCoroutine(DarkenImage(afterSignal - 1, darkenImage.color, targetAlpha));

            warningText.gameObject.SetActive(true);
            StartCoroutine(FlashWarning(warningText));

            // Change music to rain and play rain sfx
            yield return StartCoroutine(MusicManager.Instance.FadeOutMainMusic());
            yield return StartCoroutine(MusicManager.Instance.FadeInRainMusic());

            yield return darkenBefore;
            /*while (elapsed < afterSignal)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(color.a, targetAlpha, elapsed / (afterSignal - 1));
                darkenImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }*/

            warningText.gameObject.SetActive(false);
            StopCoroutine(FlashWarning(warningText));

            // apply rain effects
            Quaternion rotation = Quaternion.Euler(0f, 0f, -25f);
            rainEffect = Instantiate(rainParticles.gameObject, _camera.transform.position + new Vector3(2, 12, 25),
                rotation, _camera.transform);
            
            SoundManager.Instance.PlayRainSfx();

            ParticleSystem ps = rainEffect.GetComponent<ParticleSystem>();
            ps.Play();

            float timer = 0f;
            while (timer < duration)
            {
                if (_playerHealth != null)
                {
                    _playerHealth.TakeDamage(_rainDamage);
                    _mainTree.Damage(_rainDamage);
                }

                yield return new WaitForSeconds(damageInterval);
                timer += damageInterval;
            }

            // disable rain effects
            ps.Stop();
            SoundManager.Instance.StopRainSfx();

            Coroutine darken = StartCoroutine(DarkenImage(afterSignal - 1, darkenImage.color, color.a));
            // Change music back
            yield return StartCoroutine(MusicManager.Instance.FadeOutRainMusic());
            yield return StartCoroutine(MusicManager.Instance.FadeInMainMusic());
            yield return darken;
            Destroy(ps);
        }
    }

    private IEnumerator FlashWarning(TextMeshProUGUI warningText)
    {
        float flashDuration = 0.5f;
        Color color = warningText.color;

        float elapsedtime = 0f;
        while (elapsedtime < afterSignal)
        {
            for (float time = 0; time < flashDuration; time += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(0f, 1f, time / flashDuration);
                warningText.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }

            for (float time = 0; time < flashDuration; time += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(1f, 0f, time / flashDuration);
                warningText.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
        }

        warningText.color = new Color(color.r, color.g, color.b, 0f);
    }

    private IEnumerator DarkenImage(float duration, Color color, float targetAlpha)
    {
        float elapsed = 0;
        while (elapsed < afterSignal)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(color.a, targetAlpha, elapsed / (afterSignal - 1));
            darkenImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
    }
    public void GameOver ()
    {
        SoundManager.Instance.StopAllSfx();
        StartCoroutine(HandleGameOver());
    }

    public void GameWon()
    {
        SoundManager.Instance.StopAllSfx();
        StartCoroutine(HandleGameWon());
    }

    private IEnumerator HandleGameWon()
    {
        isGameRunning = false;
        if (_gameOver != null)
        {
            _gameOver.TriggerGameWonUI();
        }
        if (MusicManager.Instance.isRainPlaying())
        {
            yield return MusicManager.Instance.FadeOutRainMusic();
            yield return MusicManager.Instance.FadeInMainMusic();
        }
        Time.timeScale = 0f;
    }

    private IEnumerator HandleGameOver()
    {
        isGameRunning = false;
        if (_gameOver != null)
        {
            _gameOver.TriggerGameOverUI();
        }
        if (MusicManager.Instance.isMainPlaying())
        {
            yield return MusicManager.Instance.FadeOutMainMusic();
            yield return MusicManager.Instance.FadeInRainMusic();
        }
        Time.timeScale = 0f;
    }
}
