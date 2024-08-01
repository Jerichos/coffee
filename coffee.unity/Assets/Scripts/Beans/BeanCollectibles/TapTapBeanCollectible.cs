using System;
using POLYGONWARE.Coffee.Buffs;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Beans.BeanCollectibles
{

// Increases tap efficiency by [v1%] for the next [d].
public class TapTapBeanCollectible : BeanCollectible
{
    [SerializeField] private BuffSO _tapTapBeanBuff;
    
    protected override void OnCollect(PlayerManager player, ResearchedBeanData beanData)
    {
        double bonus = beanData.Rank * BeanSO.V1;
        
        Buff rubyBuff = new Buff(_tapTapBeanBuff, BeanSO.Duration, bonus, BuffTarget.TAP);
        
        player.BuffManager.AddBuff(rubyBuff);
    }
}
}