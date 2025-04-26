using UnityEngine;

public class LeafPad : MonoBehaviour
{
    [SerializeField] Sprite _idle;
    [SerializeField] Sprite _selected;
    [SerializeField] Sprite _using;

    [SerializeField] float _offsetY;

    private SpriteRenderer _spriteRenderer;
    private PlayerInputHandler _inputHandler;

    private Transform _playerTransform;

    private bool isActive = false;
    private bool isBeingUsed = false;

    private Vector3 Offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _idle;
        Offset = new Vector3(0, _offsetY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler != null && _inputHandler.InteractWasPressed && isActive == true)
        {
            Use();
            //_animationController.AnimateAttack();
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
        _spriteRenderer.sprite = _using;
        isBeingUsed = true;
    }
}
