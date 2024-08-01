using System;
using System.Numerics;
using POLYGONWARE.Coffee.Theme;
using POLYGONWARE.Coffee.Utilities;

namespace POLYGONWARE.Coffee.Player
{
// cost coffee, takes time to upgrade, locked by level, etc.
// each upgrade has a unique effect and level to scale the effect
// some upgrades are passive, some are active
public abstract class Upgrade
{
    public UpgradeSO UpgradeSO { get; private set; }

    // TODO: move to upgrade data (create one)
    // TODO: then we would have base data, and upgrade data
    // TODO: upgrade could be further enhanced by other buffs, upgrades, etc.
    public double Cooldown;
    public double Value;
    
    public uint Rank { get; private set; }
    public Action<uint> RankChangedEvent;
    
    DateTime _upgradeStartTime;
    DateTime _upgradeEndTime;
    
    public Upgrade(UpgradeSO upgradeSO)
    {
        UpgradeSO = upgradeSO;
        Rank = 1;
        
        Cooldown = upgradeSO.Cooldown;
        Value = upgradeSO.Value;
    }

    public BigInteger GetUpgradeCost()
    {
        return UpgradeSO.Cost * (Rank + 1);
    }

    public void Update(double deltaTime)
    {
        OnUpdate(deltaTime);
    }
    
    public void ApplyUpgrade(PlayerManager player)
    {
        OnApplyUpgrade(player);
    }
    
    public void RemoveUpgrade(PlayerManager player)
    {
        OnRemoveUpgrade(player);
    }
    
    public void IncreaseRank()
    {
        Rank++;
        RankChangedEvent?.Invoke(Rank);
    }
    
    protected abstract void OnUpdate(double deltaTime);
    protected abstract void OnApplyUpgrade(PlayerManager player);
    protected abstract void OnRemoveUpgrade(PlayerManager player);

    public virtual string GetFormattedDescription()
    {
        string description = UpgradeSO.Description;
        description = description.Replace("[c]", TextFormatter.ColorText(Cooldown.ToString(), ThemeManager.ColorPalette.NumberPositive, true));
        description = description.Replace("[t]", TextFormatter.ColorText((Value*100) + "%", ThemeManager.ColorPalette.NumberPositive, true));
        description = description.Replace("[v]", TextFormatter.ColorText((UpgradeSO.Value * 100) + "%", ThemeManager.ColorPalette.NumberPositive, true));
        return description;
    }
}

}