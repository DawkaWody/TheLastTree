using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LeafPad : MonoBehaviour
{
    public ItemType itemType;
    [SerializeField] Sprite _idle;
    [SerializeField] Sprite _selected;
    [SerializeField] Sprite _using;

    [SerializeField] float _offsetY;

    private SpriteRenderer _spriteRenderer;
    private PlayerInputHandler _inputHandler;
    private PlayerInventory _playerInventory;

    private Transform _playerTransform;

    private bool isActive = false;
    public bool isBeingUsed = false;

    [SerializeField] private int maxHealth = 50;
    private int currentHealth;

    public int health;

    private Vector3 Offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _idle;
        Offset = new Vector3(0, _offsetY, 0);
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = currentHealth;
        if (_inputHandler != null && _inputHandler.InteractWasPressed && isActive)
        {
            var inventory = _playerInventory;
            if (inventory != null)
            {
                if (inventory.CanPickUp(itemType))
                {
                    inventory.PickUpItem(itemType, this);
                    Use();
                }
            }
        }

        if (isBeingUsed && _playerTransform != null)
        {
            transform.position = _playerTransform.position + Offset;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _spriteRenderer.sprite = _selected;
            isActive = true;
            _playerTransform = collision.transform;
            _inputHandler = collision.GetComponent<PlayerInputHandler>();
            _playerInventory = collision.GetComponent<PlayerInventory>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _spriteRenderer.sprite = _idle;
            isActive = false;
            _playerTransform = null;
            _inputHandler = null;
        }
    }
    void Use()
    {
        SoundManager.Instance.PlayLeafPadCollectSfx();
        _spriteRenderer.sprite = _using;
        _spriteRenderer.sortingOrder = 1;
        isBeingUsed = true;
    }

    public void LeafPadTakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_playerInventory != null)
        {
            _playerInventory.ClearHeldItem(ItemType.LeafPad);
        }
        Destroy(gameObject);
    }
}
