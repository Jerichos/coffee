using System;
using POLYGONWARE.Coffee.Player;
using TMPro;
using UnityEngine;

namespace UI
{
public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    
    [SerializeField] private TMP_Text _currentCoffeesText;
    [SerializeField] private TMP_Text _totalCoffeesText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _prestigeLevelText;
    [SerializeField] private TMP_Text _coffeesToNextLevelText;
    [SerializeField] private ProgressBar _prestigeProgressBar;

    private void OnEnable()
    {
        Debug.Log("OnEnable UIManager.cs");
        _playerManager.OnCoffeesChanged += OnCoffeesChanged;
        _playerManager.OnCoffeesToNextLevelChanged += OnCoffeesToNextLevelChanged;
        _playerManager.OnLevelChanged += OnLevelChanged;
        _playerManager.OnTotalCoffeesGeneratedChanged += OnTotalCoffeesGeneratedChanged;
        
        OnCoffeesChanged(_playerManager.Coffees);
        OnCoffeesToNextLevelChanged(_playerManager.CoffeesToNextLevel);
        OnLevelChanged(_playerManager.Level);
        OnTotalCoffeesGeneratedChanged(_playerManager.TotalCoffeesGenerated);
        
        _playerManager.PrestigeManager.OnPrestigeLevelChanged += OnPrestigeLevelChanged;
        OnPrestigeLevelChanged(_playerManager.PrestigeManager.PrestigeLevel);
    }

    private void OnTotalCoffeesGeneratedChanged(uint value)
    {
        _totalCoffeesText.SetText(value.ToString());
        
        _prestigeProgressBar.SetValue((float)value / _playerManager.PrestigeManager.CoffeesToNextPrestigeLevel);
    }

    private void OnPrestigeLevelChanged(uint value)
    {
        _prestigeLevelText.SetText(value.ToString());
    }

    private void OnLevelChanged(uint value)
    {
        _levelText.SetText(value.ToString());
    }

    private void OnCoffeesToNextLevelChanged(uint value)
    {
        _coffeesToNextLevelText.SetText(value.ToString());
    }

    private void OnCoffeesChanged(uint value)
    {
        _currentCoffeesText.text = value.ToString();
    }

    private void OnDisable()
    {
        _playerManager.OnCoffeesChanged -= OnCoffeesChanged;
    }
}
}