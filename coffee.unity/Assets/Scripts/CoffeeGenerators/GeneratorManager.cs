using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Schema;

namespace POLYGONWARE.Coffee.CoffeeGenerators
{
public class GeneratorManager
{
    public Dictionary<CoffeeGeneratorSO, CoffeeGenerator> Generators { get; private set; }
    
    public Action<Dictionary<CoffeeGeneratorSO, CoffeeGenerator>> GeneratorsChangedEvent;

    public GeneratorManager()
    {
        Generators = new();
    }
    
    public double GetTotalCps()
    {
        double cps = 0;
        
        foreach (var generator in Generators)
        {
            cps += generator.Value.Cps;
        }

        return cps;
    }
    
    public void AddGenerator(CoffeeGeneratorSO generatorSO)
    {
        if (Generators.TryGetValue(generatorSO, out var generator))
        {
            generator.Buy();
        }
        else
        {
            var newGenerator = new CoffeeGenerator(generatorSO);
            Generators.Add(generatorSO, newGenerator);
            newGenerator.Buy();
        }
        
        GeneratorsChangedEvent?.Invoke(Generators);
    }
    
    public BigInteger GetCost(CoffeeGeneratorSO generatorSO)
    {
        if (!Generators.TryGetValue(generatorSO, out var generator))
            return generatorSO.Cost;

        return generator.Cost;
    }

    public uint GetUpgradeCost(CoffeeGeneratorSO generatorSO)
    {
        if (!Generators.TryGetValue(generatorSO, out var generator))
            return 0;

        return generator.LevelUpgradeCost;
    }
    
    public double GetCpsPerCost(CoffeeGeneratorSO generatorSO)
    {
        if (!Generators.TryGetValue(generatorSO, out var generator))
            return (float)((double)generatorSO.Cost / generatorSO.Cps);

        return generator.GetCpsPerCost();
    }

    public uint GetGeneratorAmount(CoffeeGeneratorSO generatorSO)
    {
        if (!Generators.TryGetValue(generatorSO, out var generator1))
            return 0;
        
        return generator1.Amount;
    }

    public bool TryUpgradeGenerator(CoffeeGeneratorSO generatorSO)
    {
        if (!Generators.TryGetValue(generatorSO, out var generator))
            return false;

        if (generator.Amount <= 0)
            return false;

        generator.LevelUpgrade();
        GeneratorsChangedEvent?.Invoke(Generators);
        return true;
    }
}
}