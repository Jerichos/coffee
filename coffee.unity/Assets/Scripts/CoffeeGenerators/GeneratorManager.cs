using System;
using System.Collections.Generic;

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
    
    public float CpsGain()
    {
        float cps = 0;
        foreach (var generator in Generators)
        {
            cps += generator.Value.CoffeePerSecond;
        }

        return cps;
    }
    
    public void AddGenerator(CoffeeGeneratorSO generatorSO)
    {
        if (Generators.TryGetValue(generatorSO, out var generator))
        {
            generator.Amount++;
        }
        else
        {
            Generators.Add(generatorSO, new CoffeeGenerator(generatorSO));
        }
        
        GeneratorsChangedEvent?.Invoke(Generators);
    }

    public uint GetGeneratorCost(CoffeeGeneratorSO generatorSO)
    {
        if (!Generators.TryGetValue(generatorSO, out var generator))
            return generatorSO.BaseCost;

        return generator.Cost;
    }

    public int GetGeneratorAmount(CoffeeGeneratorSO generator)
    {
        if (!Generators.ContainsKey(generator))
            return 0;
        
        return Generators[generator].Amount;
    }
}
}