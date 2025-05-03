using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerInventory))]
public class PlayerActionsController : MonoBehaviour
{
    [SerializeField] private Transform _interactPoint;
    [SerializeField] private float _interactRadius = 0.7f;
    [SerializeField] private LayerMask _waterLayer;
    [SerializeField] private LayerMask _treeLayer;
    [SerializeField] private float _waterAmount = 2f;
    [SerializeField] private float _treeSapAmount = 35f;

    [SerializeField] private ParticleSystem _waterParticles;

    private GameObject GatherWaterEffect;

    //private bool _hasWater;
    //private bool _hasTreeSap;

    private PlayerInputHandler _inputHandler;
    private PlayerAnimationController _animationController;
    private PlayerInventory _inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _animationController = GetComponent<PlayerAnimationController>();
        _inventory = GetComponent<PlayerInventory>();

        //_hasWater = false;
        //_hasTreeSap = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler.InteractWasPressed)
        {
            bool usedTreeSap = false;
            if (_inventory.HasItem(ItemType.TreeSap))
            {
                UseTreeSap();
                usedTreeSap = true;
            }
            if (_inventory.HasItem(ItemType.Water) && !usedTreeSap)
            {
                UseWater();
                
            }
            else CollectWater();
        }
    }

    /*public void OwnTreeSap()
    {
        _hasTreeSap = true;
    }*/

    private void CollectWater()
    {
        Collider2D water = Physics2D.OverlapCircle(_interactPoint.position, _interactRadius, _waterLayer);
        if (water == null) return;

        Debug.Log("Water collected!");
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
        GatherWaterEffect = Instantiate(_waterParticles.gameObject, _interactPoint.position, rotation);
        ParticleSystem _particleSystem = GatherWaterEffect.GetComponent<ParticleSystem>();
        _particleSystem.Play();
        SoundManager.Instance.PlayWaterCollectSfx();
        //_hasWater = true;

        _inventory.PickUpItem(ItemType.Water, null);

        StartCoroutine(DestroyParticleAfterFinish(_particleSystem));
    }

    private IEnumerator DestroyParticleAfterFinish(ParticleSystem particleSystem)
    {
        while (particleSystem.isPlaying)
        {
            yield return null;
        }
        Destroy(GatherWaterEffect);
    }

    private void UseWater()
    {
        Collider2D tree = Physics2D.OverlapCircle(_interactPoint.position, _interactRadius, _treeLayer);
        if (tree == null) return;
        Debug.Log("Tree watered");
        MainTree treeScript = tree.GetComponent<MainTree>();
        treeScript.Water(_waterAmount);
        //_hasWater = false;
        _animationController.AnimateWatering();
        SoundManager.Instance.PlayWaterUseSfx(.4f);

        _inventory.ClearHeldItem(ItemType.Water);
    }

    private void UseTreeSap()
    {
        Collider2D tree = Physics2D.OverlapCircle(_interactPoint.position, _interactRadius, _treeLayer);
        if (tree == null) return;
        Debug.Log("Tree sap used");
        MainTree treeScript = tree.GetComponent<MainTree>();
        treeScript.Protect(_treeSapAmount);
        //_hasTreeSap = false;
        _inventory.ClearHeldItem(ItemType.TreeSap);
        foreach (Transform child in transform)
        {
            if (child.name.Equals("TreeSap(Clone)"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_interactPoint.position, _interactRadius);
    }
}
