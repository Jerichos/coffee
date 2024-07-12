using POLYGONWARE.Coffee.Common;
using POLYGONWARE.Coffee.Player;
using POLYGONWARE.Coffee.Theme;
using POLYGONWARE.Coffee.Upgrades;
using POLYGONWARE.Coffee.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI.Upgrades
{
public class UpgradeDetailsUI : MonoBehaviour
{
    [SerializeField] private UpgradeShop _upgradeShop;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _requirementText;
    [SerializeField] private TMP_Text _timeLeftText;
    
    [Header("Buttons")]
    [SerializeField] private Button _upgradeButton;
    
    private UpgradeSO _upgradeSO; // serves also as upgrade identifier

    public void UpdateUpgradeDetails(UpgradeSO upgradeSO)
    {
        CancelInvoke();
        
        if (!upgradeSO)
        {
            Debug.LogError("UpgradeSO is not set!");
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        _upgradeSO = upgradeSO;
        //_nameText.text = upgradeSO.Name;

        string name = upgradeSO.Name;
        string rank = "";
        
        // is the upgrade not yet active?
        PlayerManager.LocalPlayer.UpgradeManager.ActiveUpgrades.TryGetValue(upgradeSO, out var upgrade);
        
        if (upgrade != null)
        {
            rank = " +" + upgrade.Rank;
        }
        
        _nameText.SetText(name + rank);
        _timeLeftText.SetText(TimeFormatter.FormatSeconds(Formulas.CalculateUpgradeTime(upgradeSO, upgrade?.Rank ?? 0)));
        _descriptionText.text = upgradeSO.GetFormattedDescription(upgrade);
        
        if(_upgradeShop.CanUpgrade(PlayerManager.LocalPlayer, upgradeSO, message =>
        {
            Debug.Log("message: " + message.Message);
           if (message.ReturnCode == ReturnCode.SUCCESS)
           {
               _upgradeButton.interactable = true;
               _requirementText.color = ThemeManager.ColorPalette.NumberNormal;
               _requirementText.SetText(_upgradeShop.GetUpgradeCost(PlayerManager.LocalPlayer, upgradeSO).ToString());
           }
           else if (message.ReturnCode == ReturnCode.LEVEL_TOO_LOW)
           {
               _requirementText.SetText("unlocked at level " + _upgradeSO.UnlockLevel);
               _requirementText.color = ThemeManager.ColorPalette.NumberNormal;
               _upgradeButton.interactable = false;
           }
           else if(message.ReturnCode == ReturnCode.NOT_ENOUGH_COFFEES)
           {
               _upgradeButton.interactable = false;
               _requirementText.color = ThemeManager.ColorPalette.NumberNegative;
                _requirementText.SetText(_upgradeShop.GetUpgradeCost(PlayerManager.LocalPlayer, upgradeSO).ToString());
           }
           else
           {
               Debug.LogError("ERROR: " + message.Message);
           }
        }));
        
        // is the upgrade pending?
        var upgradeState = _upgradeShop.UpgradeStates[upgradeSO];
        if (upgradeState.State == UpgradeState.PENDING)
        {
            _upgradeButton.interactable = false;
            _timeLeftText.color = ThemeManager.ColorPalette.NumberNormal;
            _timeLeftText.SetText(TimeFormatter.FormatSeconds((ulong)upgradeState.TimeLeft.TotalSeconds));
            InvokeRepeating(nameof(UpdatePendingUpgrade), 1, 1);
        }
    }
    
    private void UpdatePendingUpgrade()
    {
        var upgradeState = _upgradeShop.UpgradeStates[_upgradeSO];
        
        if (upgradeState.TimeLeft.TotalSeconds <= 0)
        {
            CancelInvoke();
            _timeLeftText.color = ThemeManager.ColorPalette.NumberNormal;
            _timeLeftText.SetText("Tap to apply upgrade");
            return;
        }
        
        _timeLeftText.SetText(TimeFormatter.FormatSeconds((ulong)upgradeState.TimeLeft.TotalSeconds));
    }

    private void BuyUpgrade()
    {
        _upgradeShop.BuyUpgrade(PlayerManager.LocalPlayer, _upgradeSO, OnUpgradeCallback);
    }

    private void OnUpgradeCallback(ReturnMessage returnMessage)
    {
        if (returnMessage.ReturnCode != ReturnCode.SUCCESS)
        {
            Debug.LogError("ERROR: " + returnMessage.Message);
            return;
        }
        
        UpdateUpgradeDetails(_upgradeSO);
    }

    private void OnEnable()
    {
        _upgradeButton.onClick.AddListener(BuyUpgrade);
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(BuyUpgrade);
    }
}
}
