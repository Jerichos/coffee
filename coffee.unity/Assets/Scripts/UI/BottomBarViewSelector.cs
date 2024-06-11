using System;
using UnityEngine;

namespace POLYGONWARE.Coffee.UI
{
public class BottomBarViewSelector : MonoBehaviour
{
    [SerializeField] private GameObject _coffeeShopView;
    [SerializeField] private GameObject _upgradesView;

    private GameObject _currentView;

    private void Awake()
    {
        _coffeeShopView.SetActive(false);
        _upgradesView.SetActive(false);
    }

    public void OpenView(GameObject view)
    {
        if (_currentView != null)
        {
            _currentView.SetActive(false);
        }

        _currentView = view;
        _currentView.SetActive(true);
    }
}
}