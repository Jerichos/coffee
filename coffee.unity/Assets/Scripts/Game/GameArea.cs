using UnityEngine;

namespace POLYGONWARE.Coffee.Game
{

// is within screen bounds and within user defined margin
public class GameArea : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    // margins are screen ration based. 0.1 means 10% of the screen
    [SerializeField] private float _leftMargin;
    [SerializeField] private float _rightMargin;
    [SerializeField] private float _topMargin;
    [SerializeField] private float _bottomMargin;

    private Vector3 _min;
    private Vector3 _max;
    
    public Vector3 Min => _min;
    public Vector3 Max => _max;
    
    public static GameArea Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (_camera == null)
        {
            _camera = Camera.main;
        }
        
        UpdateArea();
    }

    private void UpdateArea()
    {
        float width = Screen.width;
        float height = Screen.height;
        
        _min = _camera.ScreenToWorldPoint(new Vector3(width * _leftMargin, height * _bottomMargin, 0));
        _max = _camera.ScreenToWorldPoint(new Vector3(width - (width * _rightMargin), height - height * _topMargin, 0));
    }

    public bool IsWithinArea(Vector3 position)
    {
        return position.x >= _min.x && position.x <= _max.x && position.y >= _min.y && position.y <= _max.y;
    }

    private void OnDrawGizmos()
    {
        UpdateArea();
        // draw area bounds
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(_min.x, _min.y, 0), new Vector3(_max.x, _min.y, 0));
        Gizmos.DrawLine(new Vector3(_max.x, _min.y, 0), new Vector3(_max.x, _max.y, 0));
        Gizmos.DrawLine(new Vector3(_max.x, _max.y, 0), new Vector3(_min.x, _max.y, 0));
        Gizmos.DrawLine(new Vector3(_min.x, _max.y, 0), new Vector3(_min.x, _min.y, 0));
    }
}
}