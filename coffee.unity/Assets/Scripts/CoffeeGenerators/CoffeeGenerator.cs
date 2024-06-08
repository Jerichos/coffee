namespace POLYGONWARE.Coffee.CoffeeGenerators
{
public class CoffeeGenerator
{
    public GeneratorType Type;
    public int Cost => (1+Amount) * 10;
    public int CoffeePerSecond = 1;
    public int Amount;
}
}