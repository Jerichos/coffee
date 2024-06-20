using POLYGONWARE.Coffee.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Coffee.Game
{

// ping pong inside area, when hit the area, it will bounce randomly inside the area
public class PingPongInsideArea : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    
    private Transform _transform;
    
    private Vector3 _direction;
    private Vector3 _velocity;
    
    private void Awake()
    {
        _transform = transform;
        _direction = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        _velocity = _direction * (_speed * Time.deltaTime);
        _transform.position += _velocity;

        if (!CoffeeArea.Instance.IsWithinArea(_transform.position))
        {
            // get random point in area
            Vector3 randomPoint = new Vector3(Random.Range(CoffeeArea.Instance.Min.x, CoffeeArea.Instance.Max.x),
                Random.Range(CoffeeArea.Instance.Min.y, CoffeeArea.Instance.Max.y), 0);
            _direction = (randomPoint - _transform.position).normalized;
        }
    }
}
}