using System;
using System.Collections.Generic;
using POLYGONWARE.Coffee.Buffs;
using POLYGONWARE.Coffee.CoffeeGenerators;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Beans.BeanCollectibles
{

// Increases one random coffee producer production by [v1%] for [d].
public class TurboBeanCollectible : BeanCollectible
{
    [SerializeField] private BuffSO _turboBeanBuff;
    
    protected override void OnCollect(PlayerManager player, ResearchedBeanData beanData)
    {
        // pick random active generator
        var generators = player.GeneratorManager.Generators;
        var keys = new List<CoffeeGeneratorSO>(generators.Keys); // assuming GeneratorKeyType is the type of the keys
        var randomIndex = UnityEngine.Random.Range(0, keys.Count);

        var randomKey = keys[randomIndex];
        var generator = generators[randomKey];
        
        double bonus = beanData.Rank * BeanSO.V1;
        Buff buff = new Buff(_turboBeanBuff, BeanSO.Duration, bonus, BuffTarget.CPS, generator);
    }
}
}