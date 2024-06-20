using System;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
public class CoffeeCircle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LineRenderer _circleBorder;
    [SerializeField] private int _borderSegments = 12;
    [SerializeField] private float _borderWidth = 0.1f;
    [SerializeField][Range(0,1)] private float _borderFill = 0.5f; // 1 is full circle, 0.5 is half circle
    [SerializeField] private bool _borderLoop = true;
    
    [SerializeField] float _radius;

    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            UpdateCircle();
        }
    }

    private void UpdateCircle()
    {
        _spriteRenderer.transform.localScale = Vector3.one * _radius * 2;
        
        // draw line around the circle
        _circleBorder.positionCount = _borderSegments;
        _circleBorder.useWorldSpace = false;
        _circleBorder.loop = _borderLoop;
        _circleBorder.startWidth = _borderWidth;
        _circleBorder.endWidth = _borderWidth;
        
        float angleStep = (360f * _borderFill) / _borderSegments;
        
        for (int i = 0; i < _borderSegments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;
            _circleBorder.SetPosition(i, new Vector3(x, y, 0));
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Radius = _radius;
    }
#endif
}
}