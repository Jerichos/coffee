using System;
using System.Collections.Generic;
using System.Numerics;
using POLYGONWARE.Coffee.Common;
using POLYGONWARE.Coffee.Player;
using POLYGONWARE.Coffee.Player.Upgrades;
using POLYGONWARE.Coffee.Utilities;
using UnityEngine;

namespace POLYGONWARE.Coffee.UI.Upgrades
{

public enum UpgradeState
{
    INACTIVE,
    ACTIVE,
    PENDING,
    FINISHED
}

public class UpgradeShop : MonoBehaviour
{
    [SerializeField] AvailableUpgradesSO _availableUpgrades;
    public AvailableUpgradesSO AvailableUpgrades => _availableUpgrades;

    public readonly Dictionary<UpgradeSO, UpgradeStateData> UpgradeStates = new();

    //public Action<UpgradeStateData> UpgradeStateChangedEvent;
    public Action<UpgradeSO, UpgradeStateData> UpgradeStateChangedEvent;

    private void Awake()
    {
        // init default states
        foreach (var upgrade in _availableUpgrades.Upgrades)
        {
            UpgradeStates.Add(upgrade, new UpgradeStateData
            {
                State = UpgradeState.INACTIVE
            });
        }
        
        // TODO: load from save
    }

    private Upgrade GetPlayerUpgrade(PlayerManager player, UpgradeSO upgradeSO)
    {
        return player.UpgradeManager.ActiveUpgrades.GetValueOrDefault(upgradeSO);
    }

    public void BuyUpgrade(PlayerManager player, UpgradeSO upgradeSO, Action<ReturnMessage> callback)
    {
        if (!CanUpgrade(player, upgradeSO, callback))
            return;

        var upgrade = GetPlayerUpgrade(player, upgradeSO);

        // deduct coffees
        player.ConsumeCoffee(GetUpgradeCost(player, upgradeSO));

        var timeToUpgrade = Formulas.CalculateUpgradeTime(upgradeSO, upgrade?.Rank ?? 0);
        Debug.Log("timeToUpgrade: " + timeToUpgrade);
        
        // add upgrade to pending
        var newState = UpgradeStates[upgradeSO];
        newState.StartTime = DateTime.Now;
        newState.EndTime = DateTime.Now + TimeSpan.FromSeconds(timeToUpgrade);
        newState.State = UpgradeState.PENDING;
        UpgradeStates[upgradeSO] = newState;
        
        UpgradeStateChangedEvent?.Invoke(upgradeSO, newState);
        Invoke(nameof(OnFinishedPendingUpgrade), (float)newState.TimeLeft.TotalSeconds + 0.1f);
        // PendingUpgrades.Add(newState);
        // UpgradePendingEvent?.Invoke(newState);
    }
    
    private void OnFinishedPendingUpgrade()
    {
        List<UpgradeSO> finishedUpgrades = new();
        foreach (var keyValue in UpgradeStates)
        {
            if(keyValue.Value.State != UpgradeState.PENDING)
                continue;
            
            // check if upgrade is pending
            if(keyValue.Value.TimeLeft.TotalSeconds > 0)
                continue;
            
            // change state
            finishedUpgrades.Add(keyValue.Key);
            //
            // var stateData = keyValue.Value;
            // stateData.State = UpgradeState.FINISHED;
            // UpgradeStates[keyValue.Key] = stateData;
            // UpgradeStateChangedEvent?.Invoke(keyValue.Key, stateData);
        }
        
        foreach (var upgradeSO in finishedUpgrades)
        {
            var stateData = UpgradeStates[upgradeSO];
            stateData.State = UpgradeState.FINISHED;
            UpgradeStates[upgradeSO] = stateData;
            Debug.Log("Upgrade finished: " + upgradeSO.name);
            UpgradeStateChangedEvent?.Invoke(upgradeSO, stateData);
        }
    }
    
    public BigInteger GetUpgradeCost(PlayerManager player, UpgradeSO upgradeSO)
    {
        uint rank = 0;
        
        if (player.UpgradeManager.ActiveUpgrades.TryGetValue(upgradeSO, out var upgrade))
            rank = upgrade.Rank;

        return Formulas.CalculateUpgradeCost(upgradeSO, rank);
    }

    public bool CanUpgrade(PlayerManager player, UpgradeSO upgradeSO, Action<ReturnMessage> callback)
    {
        // if upgrade is not yet active, check base upgrade requirements
        if (upgradeSO.UnlockLevel > player.LevelManager.Level)
        {
            callback(new ReturnMessage { ReturnCode = ReturnCode.LEVEL_TOO_LOW, Message = "Level too low" });
            return false;
        }

        if (GetUpgradeCost(player, upgradeSO) > player.Coffees)
        {
            callback(new ReturnMessage { ReturnCode = ReturnCode.NOT_ENOUGH_COFFEES, Message = "Not enough coffees" });
            return false;
        }
        
        var state = UpgradeStates[upgradeSO];
        if(state.State == UpgradeState.PENDING)
        {
            callback(new ReturnMessage { ReturnCode = ReturnCode.UPGRADE_PENDING, Message = "Upgrade is pending" });
            return false;
        }
        
        if(state.State == UpgradeState.FINISHED)
        {
            callback(new ReturnMessage { ReturnCode = ReturnCode.UPGRADE_FINISHED, Message = "Upgrade is finished" });
            return false;
        }

        callback(new ReturnMessage { ReturnCode = ReturnCode.SUCCESS, Message = "Success" });
        return true;
    }

    public void ApplyUpgrade(PlayerManager localPlayer, UpgradeSO upgradeUpgradeSO)
    {
        var stateData = UpgradeStates[upgradeUpgradeSO];

        if (stateData.State != UpgradeState.FINISHED)
        {
            Debug.Log("Cannot apply because Upgrade is not finished yet");
            return;
        }
        
        localPlayer.UpgradeManager.AddUpgrade(upgradeUpgradeSO);
        stateData.State = UpgradeState.ACTIVE;
        UpgradeStates[upgradeUpgradeSO] = stateData;
        UpgradeStateChangedEvent?.Invoke(upgradeUpgradeSO, stateData);
    }
}

public struct UpgradeStateData
{
    public UpgradeState State;
    public DateTime StartTime;
    public DateTime EndTime;
    
    public TimeSpan TimeLeft => EndTime - DateTime.Now;
}

}