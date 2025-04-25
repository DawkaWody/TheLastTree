using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Vector2 _moveInput;
    private bool _facingRight;

    private Rigidbody2D _rigidbody;
    private PlayerInputHandler _inputHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _inputHandler = GetComponent<PlayerInputHandler>();

        _facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = _inputHandler.MoveInput.normalized;
        _rigidbody.linearVelocity = new Vector2(_moveInput.x, _moveInput.y) * _speed;

        TurnCheck();
    }

    private void TurnCheck()
    {
        switch (_moveInput.x)
        {
            case > 0:
                Turn(true);
                break;
            case < 0:
                Turn(false);
                break;
            default:
                break;
        }
    }

    private void Turn(bool side) // true = right, false = left
    {
        transform.localScale = side ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        _facingRight = side;
    }
}
