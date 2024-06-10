using System;
using POLYGONWARE.Coffee.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private TMP_Text _cps;
    [SerializeField] private ProgressBar _prestigeProgressBar;
    [SerializeField] private Button _levelUPButton;

    private void OnEnable()
    {
        Debug.Log("OnEnable UIManager.cs");
        _playerManager.OnCoffeesChanged += OnCoffeesChanged;
        _playerManager.OnCoffeesToNextLevelChanged += OnCoffeesToNextLevelChanged;
        _playerManager.OnLevelChanged += OnLevelChanged;
        _playerManager.OnTotalCoffeesGeneratedChanged += OnTotalCoffeesGeneratedChanged;
        _playerManager.OnCpsChanged += OnCpsChanged;
        
        OnCoffeesChanged(_playerManager.Coffees);
        OnCoffeesToNextLevelChanged(_playerManager.CoffeesToNextLevel);
        OnLevelChanged(_playerManager.Level);
        OnTotalCoffeesGeneratedChanged(_playerManager.TotalCoffeesGenerated);
        OnCpsChanged(_playerManager.Cps);
        
        _playerManager.PrestigeManager.OnPrestigeLevelChanged += OnPrestigeLevelChanged;
        OnPrestigeLevelChanged(_playerManager.PrestigeManager.PrestigeLevel);
    }

    private void OnCpsChanged(float amount)
    {
        _cps.SetText(amount.ToString("F1"));
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