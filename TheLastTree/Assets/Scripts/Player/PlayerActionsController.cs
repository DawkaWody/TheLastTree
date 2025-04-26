using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerActionsController : MonoBehaviour
{
    [SerializeField] private Transform _interactPoint;
    [SerializeField] private float _interactRadius = 0.7f;
    [SerializeField] private LayerMask _waterLayer;
    [SerializeField] private LayerMask _treeLayer;

    private bool _hasWater;

    private PlayerInputHandler _inputHandler;
    private PlayerAnimationController _animationController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _animationController = GetComponent<PlayerAnimationController>();

        _hasWater = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler.InteractWasPressed)
        {
            if (_hasWater)
            {
                UseWater();
            }
            else
            {
                CollectWater();
            }
        }
    }

    private void CollectWater()
    {
        Collider2D water = Physics2D.OverlapCircle(_interactPoint.position, _interactRadius, _waterLayer);
        if (water != null)
        {
            Debug.Log("Water collected!");
            _hasWater = true;
        }
    }

    private void UseWater()
    {
        Collider2D tree = Physics2D.OverlapCircle(_interactPoint.position, _interactRadius, _treeLayer);
        if (tree != null)
        {
            Debug.Log("Tree watered");
            _hasWater = false;
            _animationController.AnimateWatering();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_interactPoint.position, _interactRadius);
    }
}
