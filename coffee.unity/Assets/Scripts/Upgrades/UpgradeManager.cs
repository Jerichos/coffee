using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Upgrades
{
public class UpgradeManager
{
    //public readonly List<Upgrade> Upgrades = new();
    public readonly Dictionary<UpgradeSO, Upgrade> ActiveUpgrades = new();

    public Action<UpgradeSO, Upgrade> UpgradeChangedEvent;
    
    private readonly PlayerManager _playerManager;
    
    public UpgradeManager(PlayerManager player)
    {
        _playerManager = player;
    }
    
    public void Update(double deltaTime)
    {
        foreach (var upgrade in ActiveUpgrades.Values)
        {
            upgrade.Update(deltaTime);
        }
    }
    
    public void AddUpgrade(UpgradeSO upgradeSO)
    {
        Debug.Log("AddUpgrade: " + upgradeSO.name);
        // add upgrade if it already has upgrade, increase rank
        if (ActiveUpgrades.TryGetValue(upgradeSO, out var upgrade))
        {
            upgrade.IncreaseRank();
            UpgradeChangedEvent?.Invoke(upgradeSO, upgrade);
            return;
        }
        
        // create new upgrade
        var upgradeInstance = upgradeSO.CreateUpgradeInstance();
        ActiveUpgrades.Add(upgradeSO, upgradeInstance);
        upgradeInstance.ApplyUpgrade(_playerManager);
        UpgradeChangedEvent?.Invoke(upgradeSO, ActiveUpgrades[upgradeSO]);
    }
}
}