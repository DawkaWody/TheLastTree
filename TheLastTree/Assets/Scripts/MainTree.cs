using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(TreeDeco))]
public class MainTree : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 200f;
    [SerializeField] private float _maxWatering = 10f;
    [SerializeField] private float _growThreshold = 8f;
    [SerializeField] private float _witherThreshold = 3f;
    [SerializeField] private float _witherDamageRate = 5f;
    [SerializeField] private float _dehydrationRate = 3f;
    [SerializeField] private float _growTime = 5f;
    [SerializeField] private Sprite[] _growthStages;
    [SerializeField] private Vector2[] _growthDimensions;
    [SerializeField] private float[] _offsets;
    [SerializeField] private RectTransform _wateringBarTransform;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _wateringBar;

    private float _logTime = 1.5f;
    private float _health;
    private float _watering;
    private int _growth;
    private float _sapHp;

    private float _logTimer;
    private float _growthTimer;
    private float _witherTimer;
    private float _dehydrationTimer;

    private SpriteRenderer _spriteRenderer;
    private TreeDeco _treeDeco;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _treeDeco = GetComponent<TreeDeco>();

        _logTimer = 0f;
        _health = _maxHealth;
        _watering = _maxWatering;
        _growth = 0;

        if (_healthBar == null)
        {
            GameObject healthBarContainer = GameObject.Find("TreeHealthBar");
            foreach (Transform child in healthBarContainer.transform)
            {
                if (child.name.Equals("Fill")) _healthBar = child.GetComponent<Image>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthBar == null)
        {
            GameObject healthBarContainer = GameObject.Find("TreeHealthBar");
            if (healthBarContainer != null)
            {
                foreach (Transform child in healthBarContainer.transform)
                {
                    if (child.name.Equals("Fill")) _healthBar = child.GetComponent<Image>();
                }
            }
        }

        _logTimer += Time.deltaTime;
        if (_logTimer >= _logTime)
        {
            Debug.Log($"==Main tree report== Watering: {_watering} - growing: {_watering >= _growThreshold}" +
                      $"withering: {_watering <= _witherThreshold}; Growth stage: {_growth}/{_growthStages.Length - 1};" +
                      $"Tree health: {_health}");
            _logTimer = 0f;
        }

        _dehydrationTimer += Time.deltaTime;
        if (_dehydrationTimer >= _dehydrationRate)
        {
            _watering -= 1f;
            _dehydrationTimer = 0f;
            if (_watering < 0f)
            {
                _watering = 0f;
            }
        }

        if (_watering >= _growThreshold)
        {
            _growthTimer += Time.deltaTime;
            if (_growthTimer >= _growTime)
            {
                Grow();
                _growthTimer = 0f;
            }
        }
        if (_watering <= _witherThreshold)
        {
            _witherTimer += Time.deltaTime;
            if (_witherTimer >= _witherDamageRate)
            {
                Damage(1);
                _witherTimer = 0f;
            }
        }
        else
        {
            _witherTimer = 0f;
        }

        if (_healthBar != null) _healthBar.fillAmount = _health / _maxHealth;
        _wateringBar.fillAmount = _watering / _maxWatering;
    }

    private void Grow()
    {
        if (_growth >= _growthStages.Length - 1)
        {
            Win();
            return;
        }
        _growth++;
        if (_growth < _growthStages.Length)
        {
            _spriteRenderer.sprite = _growthStages[_growth];
            _treeDeco.width = _growthDimensions[_growth].x;
            _treeDeco.height = _growthDimensions[_growth].y;
            transform.position = new Vector3(transform.position.x, transform.position.y + _offsets[_growth]);
            _wateringBarTransform.anchoredPosition =
                new Vector2(_wateringBarTransform.anchoredPosition.x, _wateringBarTransform.anchoredPosition.y + _offsets[_growth]);
        }
    }

    public void Damage(float amount)
    {
        if (_sapHp >= amount) _sapHp -= amount;
        else if (_sapHp > 0)
        {
            _sapHp = 0f;
            _health -= amount - _sapHp;
        }
        else _health -= amount;
        if (_health <= 0) Die();
    }

    public void Water(float amount)
    {
        _watering += amount;
        _watering = Mathf.Clamp(_watering, 0f, _maxWatering);
    }

    public void Protect(float hp)
    {
        if (_sapHp > 0) return;
        _sapHp = hp;
    }

    private void Win()
    {
        if(GameManager.Instance != null) GameManager.Instance.GameWon();
    }

    private void Die()
    {
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}
