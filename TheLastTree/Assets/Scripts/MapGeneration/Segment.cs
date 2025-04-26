using UnityEngine;

public class Segment : MonoBehaviour
{
    [SerializeField] private GameObject[] _decorationVariants;
    [SerializeField] private bool _isSegment0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_isSegment0)
        {
            // Unparent the tree
            foreach (Transform child in transform)
            {
                if (child.CompareTag("MainTree"))
                {
                    child.SetParent(null);
                }
            }
        }
        if (_decorationVariants.Length == 0)
            Debug.LogWarning("No segment decoration variants detected", gameObject);
    }

    public void Decorate()
    {
        _decorationVariants[Random.Range(0, _decorationVariants.Length)].SetActive(true);
    }
}
