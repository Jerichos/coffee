using System.Numerics;
using POLYGONWARE.Coffee.Player;
using POLYGONWARE.Coffee.Utilities;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI
{
public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    
    [SerializeField] private TMP_Text _currentCoffeesText;
    [SerializeField] private TMP_Text _totalCoffeesText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _coffeesToNextLevelText;
    [SerializeField] private TMP_Text _cps;
    [SerializeField] private TMP_Text _levelPointsLeftText;
    [SerializeField] private TMP_Text _timePlayedText;
    
    [SerializeField] private ProgressBar _prestigeProgressBar;
    [SerializeField] private Button _levelUPButton;
    
    private BigInteger CoffeesToNextLevelLeft => _playerManager.LevelManager.CoffeesToNextLevel - _playerManager.TotalCoffeesGenerated;
    private decimal ProgressValue2 =>
        (decimal)(_playerManager.TotalCoffeesGenerated - _playerManager.LevelManager.GetCoffeesForPreviousLevel()) /
        (decimal)(_playerManager.LevelManager.CoffeesToNextLevel - _playerManager.LevelManager.GetCoffeesForPreviousLevel());

    private void OnEnable()
    {
        Debug.Log("OnEnable UIManager.cs");
        _playerManager.OnCoffeesChanged += OnCoffeesChanged;
        _playerManager.LevelManager.LevelChangedEvent += OnLevelChanged;
        _playerManager.OnTotalCoffeesGeneratedChanged += OnTotalCoffeesGeneratedChanged;
        _playerManager.OnCpsChanged += OnCpsChanged;
        _playerManager.PlayerStatsChangedEvent += OnPlayerStatsChanged;
        
        OnCoffeesChanged(_playerManager.Coffees);
        OnLevelChanged(_playerManager.LevelManager.Level);
        OnTotalCoffeesGeneratedChanged(_playerManager.TotalCoffeesGenerated);
        OnCpsChanged(_playerManager.Cps);
        
        _playerManager.LevelManager.LevelPointsChangedEvent += OnLevelPointsChanged;
        _playerManager.LevelManager.CanLevelUPEvent += OnCanLevelUpChanged;
        OnLevelPointsChanged(_playerManager.LevelManager.LevelPoints);
        OnCanLevelUpChanged(_playerManager.LevelManager.CanLevelUp);
    }

    private void OnPlayerStatsChanged(PlayerStats playerStats)
    {
        _timePlayedText.SetText(Util.FormatSecondsToDDHHMM(playerStats.SecondsPlayed));
    }

    private void OnLevelPointsChanged(uint value)
    {
        _levelPointsLeftText.SetText(value.ToString());
        if(value > 0)
            _levelPointsLeftText.gameObject.SetActive(true);
        else
            _levelPointsLeftText.gameObject.SetActive(false);
    }

    private void OnCanLevelUpChanged(bool value)
    {
        _levelUPButton.gameObject.SetActive(value);
        _prestigeProgressBar.gameObject.SetActive(!value);
    }

    private void OnCpsChanged(double amount)
    {
        _cps.SetText(amount.ToString("F1"));
    }

    private void OnTotalCoffeesGeneratedChanged(BigInteger value)
    {
        _totalCoffeesText.SetText(value.ToString());
        
        //float progressValue = (float) _playerManager.TotalCoffeesGenerated / _playerManager.LevelManager.CoffeesToNextLevel;
        
        // totalCofees = 15
        // coffeesToNextLevel = 20
        // coffeesToPreviousLevel = 10
        // BigInteger coffesTotalMinusPreviousLevel = _playerManager.TotalCoffeesGenerated - _playerManager.LevelManager.CoffeesToPreviousLevel;
        // BigInteger coffesToNextLevel = _playerManager.LevelManager.CoffeesToNextLevel - _playerManager.LevelManager.CoffeesToPreviousLevel;
        //float progressValue2 = (float)((decimal)coffesTotalMinusPreviousLevel / (decimal)coffesToNextLevel);
        
        _prestigeProgressBar.SetValue((float)ProgressValue2);
        //Debug.Log("progressValue: " + ProgressValue2);
        
        _coffeesToNextLevelText.SetText(CoffeesToNextLevelLeft.ToString());

        if (CoffeesToNextLevelLeft > 0)
        {
            _prestigeProgressBar.gameObject.SetActive(true);
            _levelUPButton.gameObject.SetActive(false);
        }
        else
        {
            _prestigeProgressBar.gameObject.SetActive(false);
            _levelUPButton.gameObject.SetActive(true);
        }
    }

    private void OnLevelChanged(uint value)
    {
        _prestigeProgressBar.SetValue((float)ProgressValue2);
        
        _coffeesToNextLevelText.SetText(CoffeesToNextLevelLeft.ToString());
        
        _levelText.SetText(value.ToString());
    }

    private void OnCoffeesChanged(BigInteger value)
    {
        _currentCoffeesText.text = value.ToString();
    }

    private void OnDisable()
    {
        _playerManager.OnCoffeesChanged -= OnCoffeesChanged;
    }
}
}