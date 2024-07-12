using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.Common.UI
{
// used to group of selectable elements
public class SelectionGroup : MonoBehaviour
{
    private SelectableBase _selected;
    public SelectableBase Selected => _selected;

    public void Select(SelectableBase selectable)
    {
        OnSelected(selectable, _selected);
        _selected?.Deselect();
        selectable.Select();
        _selected = selectable;
    }
    
    public void Deselect(SelectableBase selectableBase)
    {
        OnDeselected(selectableBase);
        _selected = null;
    }

    protected virtual void OnDeselected(SelectableBase selectableBase)
    {
        Debug.Log("OnDeselected");
    }

    protected virtual void OnSelected(SelectableBase newSelectable, SelectableBase prevSelectable)
    {
        Debug.Log("OnSelected");
    }

}
}