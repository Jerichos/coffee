using POLYGONWARE.Coffee.Player;
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
    [SerializeField] private TMP_Text _prestigeLevelText;
    [SerializeField] private TMP_Text _coffeesToNextLevelText;
    [SerializeField] private TMP_Text _cps;
    [SerializeField] private ProgressBar _prestigeProgressBar;
    [SerializeField] private Button _levelUPButton;
    
    private int CoffeesToNextLevelLeft => (int)(_playerManager.LevelManager.CoffeesToNextLevel - _playerManager.TotalCoffeesGenerated);

    private void OnEnable()
    {
        Debug.Log("OnEnable UIManager.cs");
        _playerManager.OnCoffeesChanged += OnCoffeesChanged;
        _playerManager.LevelManager.OnLevelChanged += OnLevelChanged;
        _playerManager.OnTotalCoffeesGeneratedChanged += OnTotalCoffeesGeneratedChanged;
        _playerManager.OnCpsChanged += OnCpsChanged;
        
        OnCoffeesChanged(_playerManager.Coffees);
        OnLevelChanged(_playerManager.LevelManager.Level);
        OnTotalCoffeesGeneratedChanged(_playerManager.TotalCoffeesGenerated);
        OnCpsChanged(_playerManager.Cps);
        
        _playerManager.LevelManager.CanLevelUPEvent += OnCanLevelUpChanged;
        OnCanLevelUpChanged(_playerManager.LevelManager.CanLevelUp);
        
        _playerManager.PrestigeManager.OnPrestigeLevelChanged += OnPrestigeLevelChanged;
        OnPrestigeLevelChanged(_playerManager.PrestigeManager.PrestigeLevel);
    }

    private void OnCanLevelUpChanged(bool value)
    {
        _levelUPButton.gameObject.SetActive(value);
        _prestigeProgressBar.gameObject.SetActive(!value);
    }

    private void OnCpsChanged(float amount)
    {
        _cps.SetText(amount.ToString("F1"));
    }

    private void OnTotalCoffeesGeneratedChanged(uint value)
    {
        _totalCoffeesText.SetText(value.ToString());
        
        float progressValue = (float) _playerManager.TotalCoffeesGenerated / _playerManager.LevelManager.CoffeesToNextLevel;
        
        // totalCofees = 15
        // coffeesToNextLevel = 20
        // coffeesToPreviousLevel = 10
        uint coffesTotalMinusPreviousLevel = _playerManager.TotalCoffeesGenerated - _playerManager.LevelManager.CookiesToPreviousLevel;
        uint coffesToNextLevel = _playerManager.LevelManager.CoffeesToNextLevel - _playerManager.LevelManager.CookiesToPreviousLevel;
        float progressValue2 = (float)coffesTotalMinusPreviousLevel / coffesToNextLevel;
        
        _prestigeProgressBar.SetValue(progressValue2);
        Debug.Log("progressValue: " + progressValue2);
        
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

    private void OnPrestigeLevelChanged(uint value)
    {
        _prestigeLevelText.SetText(value.ToString());
    }

    private void OnLevelChanged(uint value)
    {
        float progressValue = (float) _playerManager.TotalCoffeesGenerated / _playerManager.LevelManager.CoffeesToNextLevel;
        _prestigeProgressBar.SetValue(progressValue);
        
        _coffeesToNextLevelText.SetText(CoffeesToNextLevelLeft.ToString());
        
        _levelText.SetText(value.ToString());
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