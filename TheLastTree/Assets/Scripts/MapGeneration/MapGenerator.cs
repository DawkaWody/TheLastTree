using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    [SerializeField] private Transform _topLeft;
    [SerializeField] private Transform _bottomRight;
    [SerializeField] private Segment[] _segments;
    [SerializeField] private int _segmentWidth = 8;
    [SerializeField] private int _segmentHeight = 8;
    [SerializeField] private Transform _mapContainer;

    private Vector2 _topLeftPos;
    private Vector2 _bottomRightPos;
    private int _mapWidth;
    private int _mapHeight;
    private bool _seg0Spawned;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _topLeftPos = _topLeft.position;
        _bottomRightPos = _bottomRight.position;
        _mapWidth = Mathf.Abs((int)(_bottomRightPos.x - _topLeftPos.x));
        _mapHeight = Mathf.Abs((int)(_bottomRightPos.y - _topLeftPos.y));
        _seg0Spawned = false;

        Debug.Log($"Map dimensions: {_mapWidth}x{_mapHeight}");
        if (_segments.Length <= 1)
            Debug.LogWarning("Too few segments in the list. Generator may not work as intended");
        if (_mapWidth % _segmentWidth != 0 || _mapHeight % _segmentHeight != 0) 
            Debug.LogWarning($"Invalid map dimensions. Generator may not work as intended");
        if (_mapWidth % 1 != 0 || _mapHeight % 1 != 0)
            Debug.LogWarning("Map dimensions are floats. Generator may not work as intended");

        GenerateMap();
    }

    public void GenerateMap()
    {
        if (_mapContainer.childCount > 0) ClearMap();

        Vector2 startPos = new(_topLeftPos.x + _segmentWidth / 2, _topLeftPos.y - _segmentHeight / 2);
        Vector2 endPos = new(_bottomRightPos.x - _segmentWidth / 2, _bottomRightPos.y + _segmentHeight / 2);

        for (int y = (int) startPos.y; y >= endPos.y; y -= _segmentHeight)
        {
            for (int x = (int) startPos.x; x <= endPos.x; x += _segmentWidth)
            {
                GenerateSegment(x, y);
            }
        }

        if (!_seg0Spawned)
        {
            Debug.Log("Segment 0 didn't generate. Regenerating the map");
            GenerateMap();
        }
    }

    private void GenerateSegment(float x, float y)
    {
        int segId = Random.Range(_seg0Spawned ? 1 : 0, _segments.Length);
        Segment segment = Instantiate(_segments[segId], new Vector3(x, y, 0), Quaternion.identity, _mapContainer);
        segment.Decorate();

        if (segId == 0)
        {
            _seg0Spawned = true;
        }
    }

    private void ClearMap()
    {
        _seg0Spawned = false;
        foreach (Transform segment in _mapContainer)
        {
            Destroy(segment.gameObject);
        }
    }
}
