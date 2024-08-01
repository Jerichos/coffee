using System;
using System.Collections.Generic;
using POLYGONWARE.Coffee.Buffs;
using POLYGONWARE.Coffee.CoffeeGenerators;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Beans.BeanCollectibles
{

// Provides a permanent [v1%] increase in coffee production.
public class DiamondBeanCollectible : BeanCollectible
{
    protected override void OnCollect(PlayerManager player, ResearchedBeanData beanData)
    {
        // remove current bonus
        var prevBonus = (beanData.Collected - 1) * (BeanSO.V1 * beanData.Rank);
        player.CpsMultiplierBonus -= prevBonus;
        
        // calculate new bonus and add
        var newBonus = beanData.Collected * (BeanSO.V1 * beanData.Rank);
        player.CpsMultiplierBonus += newBonus;
        
        player.RecalculateCps();
        Debug.Log("diamond collected prevBonus: " + prevBonus + " newBonus: " + newBonus);
    }
}
}