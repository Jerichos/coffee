using System;
using POLYGONWARE.Coffee.Buffs;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Beans.BeanCollectibles
{

// Increases the coffee production by [v1%] from all sources for [d].
public class RubyBeanCollectible : BeanCollectible
{
    [SerializeField] private BuffSO _rubyBuffSO;
    
    protected override void OnCollect(PlayerManager player, ResearchedBeanData beanData)
    {
        // this shall create buff.
        double bonus = beanData.Rank * BeanSO.V1;

        // we take buff SO data like icon and stuff and create from it a buff.
        // but actually the Bean has an icon, we maybe can use that instead...
        Buff rubyBuff = new Buff(_rubyBuffSO, BeanSO.Duration, bonus, BuffTarget.CPS);
        
        player.BuffManager.AddBuff(rubyBuff);
    }
}
}