using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
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

    private float _health;
    private float _watering;
    private int _growth;

    private float _growthTimer;
    private float _witherTimer;
    private float _dehydrationTimer;

    private SpriteRenderer _spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _health = _maxHealth;
        _watering = _maxWatering;
        _growth = 0;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void Damage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            _health = 0;
            Destroy(gameObject);
        }
    }

    public void Water(float amount)
    {
        _watering += amount;
        Mathf.Clamp(_watering, 0f, _maxWatering);
    }
}
