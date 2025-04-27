using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerActionsController : MonoBehaviour
{
    [SerializeField] private Transform _interactPoint;
    [SerializeField] private float _interactRadius = 0.7f;
    [SerializeField] private LayerMask _waterLayer;
    [SerializeField] private LayerMask _treeLayer;
    [SerializeField] private float _waterAmount = 2f;

    [SerializeField] private ParticleSystem _waterParticles;

    private GameObject GatherWaterEffect;

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
        if (water == null) return;
        Debug.Log("Water collected!");
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
        GatherWaterEffect = Instantiate(_waterParticles.gameObject, _interactPoint.position, rotation);
        ParticleSystem _particleSystem = GatherWaterEffect.GetComponent<ParticleSystem>();
        _particleSystem.Play();
        _hasWater = true;
    }

    private void UseWater()
    {
        Collider2D tree = Physics2D.OverlapCircle(_interactPoint.position, _interactRadius, _treeLayer);
        if (tree == null) return;
        Debug.Log("Tree watered");
        MainTree treeScript = tree.GetComponent<MainTree>();
        treeScript.Water(_waterAmount);
        _hasWater = false;
        _animationController.AnimateWatering();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_interactPoint.position, _interactRadius);
    }
}
