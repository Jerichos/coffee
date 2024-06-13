using System;
using UnityEngine;

namespace POLYGONWARE.Coffee.UI
{
public class MenuManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _menuViews;
    [SerializeField] private GameObject _defaultActiveView;

    private GameObject _currentView;

    private void Awake()
    {
        foreach (var view in _menuViews)
        {
            view.SetActive(false);
        }
        
        OpenView(_defaultActiveView);
    }

    public void OpenView(GameObject view)
    {
        if (_currentView != null)
        {
            _currentView.SetActive(false);
        }

        if (view == _currentView)
        {
            _currentView.SetActive(false);
            _currentView = null;
            return;
        }

        _currentView = view;
        _currentView.SetActive(true);
    }
}
}