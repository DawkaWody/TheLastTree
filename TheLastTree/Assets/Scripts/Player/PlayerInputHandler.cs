using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _attackActionName = "Attack";
    [SerializeField] private string _interactActionName = "Interact";

    [HideInInspector] public Vector2 MoveInput { get; private set; }
    [HideInInspector] public bool AttackWasPressed { get; private set; }
    [HideInInspector] public bool InteractWasPressed { get; private set; }

    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _interactAction;

    private PlayerInput _playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions[_moveActionName];
        _attackAction = _playerInput.actions[_attackActionName];
        _interactAction = _playerInput.actions[_interactActionName];
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();

        AttackWasPressed = _attackAction.WasPressedThisFrame();
        InteractWasPressed = _interactAction.WasPressedThisFrame();
    }
}
