using System;
using System.Collections.Generic;
using POLYGONWARE.Coffee.CoffeeGenerators;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
[DefaultExecutionOrder(-10)]
public class PlayerManager : MonoBehaviour
{
    public uint Coffees { get; private set; }
    public uint TotalCoffeesGenerated { get; private set; }
    public float Cps { get; private set; }
    
    public LevelManager LevelManager { get; private set; }
    public PrestigeManager PrestigeManager { get; private set; }
    public GeneratorManager GeneratorManager { get; private set; }
    
    public Action<uint> OnCoffeesChanged;
    public Action<uint> OnTotalCoffeesGeneratedChanged;
    public Action<float> OnCpsChanged;

    private float _cookiesTemp;

    private void Awake()
    {
        Debug.Log("PlayerManager Awake");
        LevelManager = new LevelManager(this);
        PrestigeManager = new PrestigeManager();
        GeneratorManager = new GeneratorManager();
    }

    private void Update()
    {
        _cookiesTemp += Cps * Time.deltaTime;
        
        if (_cookiesTemp >= 1)
        {
            AddGeneratedCoffee((uint) _cookiesTemp);
            
            // safe remainder
            _cookiesTemp -= (uint) _cookiesTemp;
        }
    }

    private void OnEnable()
    {
        GeneratorManager.GeneratorsChangedEvent += OnGeneratorsChanged;
        OnGeneratorsChanged(GeneratorManager.Generators);
    }

    private void OnGeneratorsChanged(Dictionary<CoffeeGeneratorSO, CoffeeGenerator> generatorManagerGenerators)
    {
        Cps = GeneratorManager.CpsGain();
        OnCpsChanged?.Invoke(Cps);
    }

    private void OnDisable()
    {
        GeneratorManager.GeneratorsChangedEvent -= OnGeneratorsChanged;
    }

    public void TapTap()
    {
        Debug.Log("gain 1 coffee");
        AddGeneratedCoffee(1);
    }
    
    public void ConsumeCoffee(uint amount)
    {
        if (Coffees < amount)
            return;
        
        Coffees -= amount;
        OnCoffeesChanged?.Invoke(Coffees);
    }
    
    public void LevelUP()
    {
        LevelManager.LevelUp();
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
