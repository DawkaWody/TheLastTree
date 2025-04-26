using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TreeDeco : MonoBehaviour
{
    public float width;
    public float height;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _alphaOnFade = 0.4f;

    private Vector2 _position;

    private Transform _player;
    private SpriteRenderer _spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _position = (Vector2)transform.position + _offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null) return;
        Color c = _spriteRenderer.color;

        if (_player.position.x > _position.x - width / 2 &&
            _player.position.x < _position.x + width / 2 &&
            _player.position.y > _position.y - height / 2 &&
            _player.position.y < _position.y + height / 2)
        {
            _spriteRenderer.color = new Color(c.r, c.g, c.b, _alphaOnFade);
        }
        else
        {
            _spriteRenderer.color = new Color(c.r, c.g, c.b, 1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector2 position = (Vector2)transform.position + _offset;
        Gizmos.DrawLine(position, position + new Vector2(width / 2, 0f));
        Gizmos.DrawLine(position, position - new Vector2(width / 2, 0f));
        Gizmos.DrawLine(position, position + new Vector2(0f, height / 2));
        Gizmos.DrawLine(position, position - new Vector2(0f, height / 2));
    }
}
