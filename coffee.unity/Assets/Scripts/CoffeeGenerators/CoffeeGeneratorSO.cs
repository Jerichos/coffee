using UnityEngine;

namespace POLYGONWARE.Coffee.CoffeeGenerators
{
[CreateAssetMenu(fileName = "NewCoffeeGenerator", menuName = "data/coffee generator", order = 0)]
public class CoffeeGeneratorSO : ScriptableObject
{
    [SerializeField] private CoffeeGeneratorData _data;
    public CoffeeGeneratorData Data => _data;

    public void SetData(CoffeeGeneratorData data)
    {
        _data = data;
    }
}
}