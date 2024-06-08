using UnityEngine;

namespace POLYGONWARE.Coffee.CoffeeGenerators
{
[CreateAssetMenu(fileName = "CoffeeGeneratorTable", menuName = "data/CoffeeGeneratorTable", order = 0)]
public class CoffeeGeneratorTable : ScriptableObject
{
    public CoffeeGeneratorSO[] Generators;
}
}