﻿using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI
{
public class LongPressButton : Button
{
    private float _invokesPerSecond = 8f;
    private float _timeToHold = 0.5f;
    
    // long press button for automatically buying generators when holding the button
    private bool _isPressed;
    private float _timePressed;
    private float _timeToInvokeT;
    private float _timeToInvoke;
    
    private void Update()
    {
        if (_isPressed)
        {
            _timeToInvoke = 1f / _invokesPerSecond;
            _timePressed += Time.deltaTime;
            
            if (_timePressed >= _timeToHold)
            {
                _timeToInvokeT += Time.deltaTime;
                Debug.Log("timeInvokeT: " + _timeToInvokeT + " timeInvoke: " + _timeToInvoke);
                
                if (_timeToInvokeT >= _timeToInvoke)
                {
                    Debug.Log("Long press detected!");
                    onClick.Invoke();
                    _timeToInvokeT = 0;
                }
            }
        }
    }
    
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        _isPressed = true;
    }
    
    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        _isPressed = false;
        _timePressed = 0;
        _timeToInvokeT = 0;
    }
    
    public override void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        _isPressed = false;
        _timePressed = 0;
        _timeToInvokeT = 0;
    }
    
    public override void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _isPressed = false;
        _timePressed = 0;
        _timeToInvokeT = 0;
    }
    
    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        _isPressed = false;
        _timePressed = 0;
        _timeToInvokeT = 0;
    }
    
    public override void OnSubmit(UnityEngine.EventSystems.BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        _isPressed = false;
        _timePressed = 0;
        _timeToInvokeT = 0;
    }
    
    public override void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        base.OnSelect(eventData);
        _isPressed = false;
        _timePressed = 0;
        _timeToInvokeT = 0;
    }
    
    public override void OnDeselect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        _isPressed = false;
        _timePressed = 0;
        _timeToInvokeT = 0;
    }
    
    public override void OnMove(UnityEngine.EventSystems.AxisEventData eventData)
    {
        base.OnMove(eventData);
        _isPressed = false;
        _timePressed = 0;
        _timeToInvokeT = 0;
    }
}
}