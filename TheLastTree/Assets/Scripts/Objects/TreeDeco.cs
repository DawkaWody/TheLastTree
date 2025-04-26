using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TreeDeco : MonoBehaviour
{
    public float width;
    public float height;
    [SerializeField] private float _alphaOnFade;

    private Transform _player;
    private SpriteRenderer _spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null) return;
        Color c = _spriteRenderer.color;

        if (_player.position.x > transform.position.x - width / 2 &&
            _player.position.x < transform.position.x + width / 2 &&
            _player.position.y > transform.position.y - height / 2 &&
            _player.position.y < transform.position.y + height / 2)
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
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(width / 2, 0f));
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(width / 2, 0f));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0f, height / 2));
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(0f, height / 2));
    }
}
