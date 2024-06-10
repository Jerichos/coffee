using UnityEngine;

namespace POLYGONWARE.Coffee.CoffeeGenerators
{
[CreateAssetMenu(fileName = "NewCoffeeGenerator", menuName = "data/coffee generator", order = 0)]
public class CoffeeGeneratorSO : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public string Description;
    public uint BaseCost;
    public float BaseCps;
    public float CostMultiplier;
}
}