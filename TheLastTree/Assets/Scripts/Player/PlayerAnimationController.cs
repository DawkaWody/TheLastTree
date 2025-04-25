using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimateMovement(Vector2 moveDirection)
    {
        if (moveDirection.sqrMagnitude <= 0)
        {
            _animator.SetBool("isMoving", false);
            return;
        }
        _animator.SetBool("isMoving", true);
        _animator.SetFloat("directionX", moveDirection.x);
        _animator.SetFloat("directionY", moveDirection.y);
    }

    public void AnimateAttack()
    {
        _animator.SetTrigger("attack");
    }

    public void AnimateDeath()
    {
        _animator.SetTrigger("die");
    }
}
