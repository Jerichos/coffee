using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.Common.UI
{
public abstract class SelectableBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator _animator;
    
    private bool _selected;
    public bool Selected => _selected;
    
    protected Animator Animator => _animator;
    
    private static readonly int SelectedBoolHash = Animator.StringToHash("selected");
    private static readonly int HoveredBoolHash = Animator.StringToHash("hovered");

    public void Select()
    {
        _selected = true;
        _animator?.SetBool(SelectedBoolHash, true);
        OnSelected();
    }
    
    public void Deselect()
    {
        _selected = false;
        _animator?.SetBool(SelectedBoolHash, false);
        OnDeselected();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var selectorGroup = GetComponentInParent<SelectionGroup>();
        
        if (selectorGroup == null)
        {
            Debug.LogError("SelectableBase: SelectionGroup not found!");
            return;
        }
        
        selectorGroup.Select(this);
    }
    
    protected virtual void OnDeselected()
    {
        Debug.Log("OnDeselected");
    }

    protected virtual void OnSelected()
    {
        Debug.Log("OnSelected");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator?.SetBool(HoveredBoolHash, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator?.SetBool(HoveredBoolHash, false);
    }
}
}