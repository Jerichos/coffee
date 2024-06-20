using System;
using POLYGONWARE.Coffee.Buffs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI
{
public class BuffUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _iconImage;
    
    // radiant filled image to show the remaining time of the buff
    [SerializeField] private Image _timerImage;

    private Buff _buff;

    public void Update()
    {
        // update timer image fill amount
        _timerImage.fillAmount = _buff.TimeLeft / _buff.BuffSO.Duration;
    }

    public void Initialize(Buff buff)
    {
        //_iconImage.sprite = buff.BuffSO.Icon;
        _buff = buff;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Buff clicked!");
        
        // TODO: show buff description via tooltip
    }
}
}