using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;

namespace POLYGONWARE.Coffee.CoffeeGenerators
{
[CreateAssetMenu(fileName = "NewCoffeeGenerator", menuName = "data/coffee generator", order = 0)]
public class CoffeeGeneratorSO : ScriptableObject
{
    public string ID;
    public Sprite Icon;
    public string Name;
    public string Description;
    public string CostString; // BigInteger
    public double Cps;
    public double CostMultiplierPerAmount;
    
    public BigInteger Cost => BigInteger.Parse(CostString);
    
    //public ulong BaseCost2 => Generation == 0? BaseCost : (ulong)Math.Pow(CostMultiplierPerGen, Generation - 1) * BaseCost;
    
    // we calculate base cost in table sheet
    // public BigInteger CalculateBaseCost2()
    // {
    //     return Generation == 0? BaseCost : (BigInteger)Math.Pow(CostMultiplierPerGen, Generation - 1) * BaseCost;
    // }
}
}