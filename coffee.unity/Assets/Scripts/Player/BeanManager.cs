using System;
using System.Collections.Generic;
using POLYGONWARE.Coffee.Beans;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
public class BeanManager
{
    private PlayerManager _player;
    
    public Dictionary<BeanSO, ResearchedBeanData> ResearchedBeans { get; private set; } = new();
    public Action<Dictionary<BeanSO, ResearchedBeanData>> ResearchedBeansChangedEvent;
    
    public BeanManager(PlayerManager playerManager)
    {
        _player = playerManager;
        // TODO: load from save, what beans are researched, and how many of them are collected, and what rank they are
    }

    public void CollectBean(Collectible collectible)
    {
        if (collectible is not BeanCollectible beanCollectible)
        {
            Debug.LogError("Collectible is not a BeanCollectible.");
            return;
        }
        
        // check if is already researched, if not return
        if (!ResearchedBeans.TryGetValue(beanCollectible.BeanSO, out var researchedBeanData))
        {
            Debug.Log("Bean not researched.");
            return;
        }
        
        // add collected beans
        researchedBeanData.Collected++;
        ResearchedBeans[beanCollectible.BeanSO] = researchedBeanData;
        
        // apply effect
        beanCollectible.Collect(_player, researchedBeanData);
    }
}

public struct ResearchedBeanData
{
    public uint Rank;
    
    /// <summary>
    /// Adjusted before applying the effect.
    /// </summary>
    public uint Collected;
}
}