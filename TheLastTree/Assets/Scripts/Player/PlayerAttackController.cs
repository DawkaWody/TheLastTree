using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private float _attackAnimationLength;

    private bool _canAttack;

    private PlayerInputHandler _inputHandler;
    private PlayerAnimationController _animationController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _animationController = GetComponent<PlayerAnimationController>();

        _canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler.AttackWasPressed && _canAttack)
        {
            _animationController.AnimateAttack();
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackAnimationLength);
        _canAttack = true;
    }
}
