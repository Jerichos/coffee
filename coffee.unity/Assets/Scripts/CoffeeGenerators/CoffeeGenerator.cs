using System;
using System.Numerics;
using UnityEngine;

namespace POLYGONWARE.Coffee.CoffeeGenerators
{

// MARTIANS love your coffee
// Make coffee not war

public class CoffeeGenerator
{
    public double Cps => (GeneratorSO.Cps * Amount) * TotalLevelUpgradesCpsGain;
    public BigInteger Cost => Amount == 0 ? GeneratorSO.Cost : (BigInteger)(Math.Pow(GeneratorSO.CostMultiplierPerAmount, Amount) * (double)GeneratorSO.Cost);
    public uint LevelUpgradeCost => Level + 1;
    
    public uint Amount {get; private set;}
    public uint Level;
    
    //public double CpsPerCost => ((double)(Cost / (GeneratorSO.Cps + TotalLevelUpgradesCpsGain);
    public readonly double PerLevelCpsGain = 1.01f; // 1% increase per level 
    public double TotalLevelUpgradesCpsGain => Math.Pow(PerLevelCpsGain, Level);
    
    public CoffeeGeneratorSO GeneratorSO { get; private set; }

    public CoffeeGenerator(CoffeeGeneratorSO generatorSO)
    {
        GeneratorSO = generatorSO;
        Level = 0;
        Amount = 0;
        Debug.Log("BaseCost: " + Cost + " SOCost: " + generatorSO.Cost);
    }

    public void LevelUpgrade()
    {
        var cpsGainBefore = TotalLevelUpgradesCpsGain;
        var cpsBefore = Cps;
        Level++;
        Debug.Log("level upgrade, increased cps gain from " + cpsGainBefore + " to " + TotalLevelUpgradesCpsGain);
        Debug.Log("level upgrade, increased cps from " + cpsBefore + " to " + Cps);
    }
    
    public void Buy()
    {
        var buyCost = Cost;
        var prevAmount = Amount;
        Amount++;
        var newCost = Cost;
        Debug.Log("bought generator, increased amount from " + prevAmount + " to " + Amount);
        Debug.Log("bought generator, increased cost from " + buyCost + " to " + newCost);
    }

    public double GetCpsPerCost()
    {
        // When Amount is 0, prevent division by zero and return a default or max value
        if (Amount == 0) 
            return double.MaxValue;

        // Calculate the current CPS
        double currentCps = GeneratorSO.Cps * TotalLevelUpgradesCpsGain;

        // Use BigInteger cost for accurate cost calculation
        BigInteger currentCost = Cost;

        // Calculate CPS per Cost
        double cpsPerCost = (double)currentCost / currentCps;
        
        Debug.Log("CPS per Cost: " + cpsPerCost + " Current CPS: " + currentCps + " Current Cost: " + currentCost + " TotalLevelUpgradesCpsGain: " + TotalLevelUpgradesCpsGain);

        return cpsPerCost;
    }
}
}