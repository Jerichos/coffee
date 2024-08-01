using System;
using System.Numerics;
using POLYGONWARE.Coffee.Game;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player.Upgrades
{

// after 5 seconds of tapping (any tap type), you get bonus of 5% (per rank) of accumulated coffees generated in the last 5 seconds
// TODO: rename to BombTapUpgrade
public class CoffeeBombUpgrade : Upgrade
{
    private double _t;
    
    private BigInteger _accumulatedCoffees;
    
    public double TimeToBonus => UpgradeSO.Cooldown;
    public double BonusPercentage => Rank * UpgradeSO.Value;
    
    private AudioClip BonusSfx => UpgradeSO.SoundClue1;
    
    public CoffeeBombUpgrade(UpgradeSO upgradeSO) : base(upgradeSO)
    {
    }

    protected override void OnUpdate(double deltaTime)
    {
        _t -= deltaTime;
        
        if (_t <= 0)
        {
            _t = TimeToBonus;
            
            if(_accumulatedCoffees <= 0)
                return;
            
            BigInteger bonus = (BigInteger)(Math.Ceiling((double)_accumulatedCoffees * BonusPercentage));
            Debug.Log("PowerTapUpgrade bonus: " + bonus + " accumulated: " + _accumulatedCoffees + " percentBonus: " + BonusPercentage * 100 + "%");
            PlayerManager.LocalPlayer.AddCoffeeBonus(bonus);
            _accumulatedCoffees = 0;
            AudioManager.PlaySfx(BonusSfx);
        }
    }

    protected override void OnApplyUpgrade(PlayerManager player)
    {
        player.TapTapEvent += OnTap;
    }

    protected override void OnRemoveUpgrade(PlayerManager player)
    {
        player.TapTapEvent -= OnTap;
    }
    
    // if upgrade is not in progress, it should start accumulating coffees
    private void OnTap(BigInteger coffeesByTap)
    {
        _accumulatedCoffees += coffeesByTap;
    }
}
}