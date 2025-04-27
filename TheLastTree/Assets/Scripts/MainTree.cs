using UnityEngine;

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

    private float _logTime = 1.5f;
    private float _health;
    private float _watering;
    private int _growth;

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
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void Grow()
    {
        if (_growth >= _growthStages.Length - 1) return;
        _growth++;
        _spriteRenderer.sprite = _growthStages[_growth];
        _treeDeco.width = _growthDimensions[_growth].x;
        _treeDeco.height = _growthDimensions[_growth].y;
        transform.position = new Vector3(transform.position.x, transform.position.y + _offsets[_growth]);
    }

    public void Damage(float amount)
    {
        _health -= amount;
        if (_health <= 0) Die();
    }

    public void Water(float amount)
    {
        _watering += amount;
        Mathf.Clamp(_watering, 0f, _maxWatering);
    }

    private void Die()
    {

    }
}
