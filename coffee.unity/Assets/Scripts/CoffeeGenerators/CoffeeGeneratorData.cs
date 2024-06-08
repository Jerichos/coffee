using System;
using UnityEngine;

namespace POLYGONWARE.Coffee.CoffeeGenerators
{
[Serializable]
public struct CoffeeGeneratorData
{
    public Sprite Icon;
    public string Name;
    public string Description;
    public float BaseCost;
    public float BaseCps;
    public float CostMultiplier;
}
}