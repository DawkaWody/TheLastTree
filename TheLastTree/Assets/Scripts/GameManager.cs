using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerHealth _playerHealth;

    [SerializeField] private int afterSignal;

    [SerializeField] private int _timeBeforeMin;
    [SerializeField] private int _timeBeforeMax;
    [SerializeField] private int _durationMin;
    [SerializeField] private int _durationMax;

    [SerializeField] private float damageInterval;
    [SerializeField] private int _rainDamage;
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

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartGame()
    {
        if (MapGenerator.Instance != null)
        {
            MapGenerator.Instance.GenerateMap();
        }
        StartCoroutine(RainCycle());
    }

    private IEnumerator RainCycle()
    {
        int timeBefore = Random.Range(_timeBeforeMin, _timeBeforeMax);
        int duration = Random.Range(_durationMin, _durationMax);
        yield return new WaitForSeconds(timeBefore - 5);
        Debug.Log("close");
        // signal for storm
        yield return new WaitForSeconds(afterSignal);
        Debug.Log("rain");
        // apply rain effects
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
        Debug.Log(" no rain");
        // disable rain effects
    }
}
