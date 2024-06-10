namespace POLYGONWARE.Coffee.CoffeeGenerators
{

// MARTIANS love your coffee

public class CoffeeGenerator
{
    public uint Cost => (uint)(GeneratorSO.BaseCost + ((Amount * GeneratorSO.CostMultiplier) * GeneratorSO.BaseCost));
    public float CoffeePerSecond => GeneratorSO.BaseCps * Amount;
    public int Amount;
    
    public CoffeeGeneratorSO GeneratorSO { get; private set; }
    
    public CoffeeGenerator(CoffeeGeneratorSO generatorSO)
    {
        GeneratorSO = generatorSO;
        Amount = 1;
    }
}
}