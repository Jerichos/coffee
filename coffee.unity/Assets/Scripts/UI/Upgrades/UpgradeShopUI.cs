using System;
using System.Numerics;
using POLYGONWARE.Coffee.Common.UI;
using POLYGONWARE.Coffee.Player;
using POLYGONWARE.Coffee.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI.Upgrades
{
public class UpgradeShopUI : SelectionGroup
{
    [SerializeField] private UpgradeShop _upgradeShop;

    [SerializeField] private UpgradeDetailsUI _detailsPanel;
    [SerializeField] private Button _upgradeButton;

    // private UpgradeUI[] _upgradeUIs;
    
    private UpgradeSO _selectedUpgrade;
    
    private void OnEnable()
    {
        Debug.Log("UpgradeShopUI OnEnable");

        foreach (var keyValue in _upgradeShop.UpgradeStates)
            OnUpgradeStateChanged(keyValue.Key, keyValue.Value);
        
        _upgradeShop.UpgradeStateChangedEvent += OnUpgradeStateChanged;
        
        PlayerManager.LocalPlayer.LevelManager.LevelChangedEvent += OnLevelChanged;
        PlayerManager.LocalPlayer.OnCoffeesChanged += OnCoffeesChanged;
        
        OnSelected(null, null);
    }

    private void OnCoffeesChanged(BigInteger value)
    {
        _detailsPanel.UpdateUpgradeDetails(_selectedUpgrade);
    }

    private void OnLevelChanged(uint value)
    {
        _detailsPanel.UpdateUpgradeDetails(_selectedUpgrade);
    }

    private void OnUpgradeStateChanged(UpgradeSO upgradeSO, UpgradeStateData upgradeStateData)
    {
        var upgradeUIs = GetComponentsInChildren<UpgradeUI>(true);
        
        for (int i = 0; i < upgradeUIs.Length; i++)
        {
            if (upgradeUIs[i].UpgradeSO == upgradeSO)
            {
                upgradeUIs[i].SetState(upgradeStateData);

                if (_selectedUpgrade == upgradeSO)
                    _detailsPanel.UpdateUpgradeDetails(_selectedUpgrade);
                
                return;
            }
        }
        
        Debug.LogError("Upgrade not found: " + upgradeSO.Name);
    }

    private void OnDisable()
    {
        _upgradeShop.UpgradeStateChangedEvent -= OnUpgradeStateChanged;
        PlayerManager.LocalPlayer.LevelManager.LevelChangedEvent -= OnLevelChanged;
        PlayerManager.LocalPlayer.OnCoffeesChanged -= OnCoffeesChanged;
    }

    protected override void OnSelected(SelectableBase newSelectable, SelectableBase prevSelectable)
    {
        if (newSelectable is not UpgradeUI upgradeUI)
        {
            Debug.LogError("newSelectable is not UpgradeUI");
            return;
        }
        
        // if upgrade is finished state, apply the upgrade
        var upgradeState = _upgradeShop.UpgradeStates[upgradeUI.UpgradeSO];
        if (upgradeState.State == UpgradeState.FINISHED)
        {
            _upgradeShop.ApplyUpgrade(PlayerManager.LocalPlayer, upgradeUI.UpgradeSO);
        }
        else
        {
            Debug.LogError("cannot apply upgrade because its not in finished state: " + upgradeState.State);
        }
        
        _selectedUpgrade = upgradeUI.UpgradeSO;
        _detailsPanel.UpdateUpgradeDetails(_selectedUpgrade);
    }
}
}