using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    public static GameManager Instance;
    private PlayerHealth _playerHealth;

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

    private GameObject rainEffect;

    private bool isGameRunning = false;
    void Awake()
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerHealth = player.GetComponent<PlayerHealth>();
        }

        StartGame(player.transform);
    }

    private void StartGame(Transform player)
    {
        if (MapGenerator.Instance != null)
        {
            MapGenerator.Instance.GenerateMap();
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
            float elapsed = 0f;
            Color color = darkenImage.color;

            warningText.gameObject.SetActive(true);
            StartCoroutine(FlashWarning(warningText));
            while (elapsed < afterSignal)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(color.a, targetAlpha, elapsed / (afterSignal - 1));
                darkenImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
            warningText.gameObject.SetActive(false);
            StopCoroutine(FlashWarning(warningText));

            // apply rain effects
            Quaternion rotation = Quaternion.Euler(0f, 0f, -25f);
            rainEffect = Instantiate(rainParticles.gameObject, _camera.transform.position + new Vector3(2, 12, 25), rotation, _camera.transform);

            ParticleSystem ps = rainEffect.GetComponent<ParticleSystem>();
            ps.Play();

            float timer = 0f;
            while (timer < duration)
            {
                if (_playerHealth != null)
                {
                    _playerHealth.TakeDamage(_rainDamage);
                }

                yield return new WaitForSeconds(damageInterval);
                timer += damageInterval;
            }

            // disable rain effects
            ps.Stop();
            elapsed = 0f;

            while (elapsed < afterSignal)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(targetAlpha, color.a, elapsed / (afterSignal - 1));
                darkenImage.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }
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
}
