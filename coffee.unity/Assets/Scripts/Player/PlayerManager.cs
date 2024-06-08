using System;
using POLYGONWARE.Coffee.CoffeeGenerators;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
[DefaultExecutionOrder(-10)]
public class PlayerManager : MonoBehaviour
{
    public uint Coffees;

    // current playthru stats
    public uint TotalCoffeesGenerated;
    public uint Level;
    public uint CoffeesToNextLevel;
    
    public PrestigeManager PrestigeManager { get; private set; }
    public GeneratorManager GeneratorManager { get; private set; }
    
    public Action<uint> OnCoffeesChanged;
    public Action<uint> OnTotalCoffeesGeneratedChanged;
    public Action<uint> OnCoffeesToNextLevelChanged;
    public Action<uint> OnLevelChanged;

    private void Awake()
    {
        Debug.Log("PlayerManager Awake");
        PrestigeManager = new PrestigeManager();
    }

    public void TapTap()
    {
        Debug.Log("gain 1 coffee");
        AddGeneratedCoffee(1);
    }
    
    public void AddGeneratedCoffee(uint amount)
    {
        Coffees += amount;
        TotalCoffeesGenerated += amount;
        
        OnCoffeesChanged?.Invoke(Coffees);
        OnTotalCoffeesGeneratedChanged?.Invoke(TotalCoffeesGenerated);
    }
}
}
