using System;
using System.Collections.Generic;
using System.Numerics;
using POLYGONWARE.Coffee.CoffeeGenerators;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
[DefaultExecutionOrder(-10)]
public class PlayerManager : MonoBehaviour
{
    public BigInteger Coffees { get; private set; }
    public BigInteger TotalCoffeesGenerated { get; private set; }
    public double Cps { get; private set; }
    
    public LevelManager LevelManager { get; private set; }
    public PrestigeManager PrestigeManager { get; private set; }
    public GeneratorManager GeneratorManager { get; private set; }
    
    public Action<BigInteger> OnCoffeesChanged;
    public Action<BigInteger> OnTotalCoffeesGeneratedChanged;
    public Action<double> OnCpsChanged;
    public Action<PlayerStats> PlayerStatsChangedEvent;

    public PlayerStats PlayerStats;

    private double _cookiesTemp;

    private void Awake()
    {
        Debug.Log("PlayerManager Awake");
        LevelManager = new LevelManager(this);
        PrestigeManager = new PrestigeManager();
        GeneratorManager = new GeneratorManager();
    }

    private void Update()
    {
        _cookiesTemp += Cps * (double)Time.deltaTime * GameManager.Instance.FastForward;
        
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
        InvokeRepeating(nameof(OnSecondTick), 1, 1);
    }
    
    private void OnSecondTick()
    {
        PlayerStats.SecondsPlayed += (ulong)(1 * GameManager.Instance.FastForward);
        PlayerStatsChangedEvent?.Invoke(PlayerStats);
        //Debug.Log("OnSecondTick, seconds played: " + PlayerStats.SecondsPlayed);
    }

    private void OnGeneratorsChanged(Dictionary<CoffeeGeneratorSO, CoffeeGenerator> generatorManagerGenerators)
    {
        Cps = GeneratorManager.GetTotalCps();
        OnCpsChanged?.Invoke(Cps);
    }

    private void OnDisable()
    {
        CancelInvoke();
        GeneratorManager.GeneratorsChangedEvent -= OnGeneratorsChanged;
    }

    public void TapTap()
    {
        AddGeneratedCoffee(1);
        PlayerStats.TapTapCount++;
        Debug.Log("TapTaps: " + PlayerStats.TapTapCount);
    }
    
    public void ConsumeCoffee(BigInteger amount)
    {
        if (Coffees < amount)
            return;
        
        Coffees -= amount;
        OnCoffeesChanged?.Invoke(Coffees);
    }
    
    public void LevelUP()
    {
        LevelManager.LevelUp();

        if (LevelManager.Level > PlayerStats.MaxLevelReached)
        {
            PlayerStats.MaxLevelReached = LevelManager.Level;
            Debug.Log("new max level reached: " + PlayerStats.MaxLevelReached);
        }
    }
    
    public void AddGeneratedCoffee(uint amount)
    {
        Coffees += amount;
        TotalCoffeesGenerated += amount;
        
        OnCoffeesChanged?.Invoke(Coffees);
        OnTotalCoffeesGeneratedChanged?.Invoke(TotalCoffeesGenerated);
    }

    public void UpgradeGenerator(CoffeeGeneratorSO generatorSO, Action<bool> callback)
    {
        if (LevelManager.LevelPoints <= 0)
        {
            Debug.Log("Not enough level points to upgrade.");
            callback?.Invoke(false);
            return;
        }
        
        if(GeneratorManager.GetGeneratorAmount(generatorSO) <= 0)
        {
            Debug.Log("No generator to upgrade.");
            callback?.Invoke(false);
            return;
        }
        
        uint upgradeCost = GeneratorManager.GetUpgradeCost(generatorSO);

        if (upgradeCost > LevelManager.LevelPoints)
        {
            Debug.Log("Not enough level points to upgrade " + generatorSO.Name + ". Required: " + upgradeCost + ", available: " + LevelManager.LevelPoints);
            callback?.Invoke(false);
            return;
        }

        if (!GeneratorManager.TryUpgradeGenerator(generatorSO))
        {
            Debug.Log("Failed to upgrade " + generatorSO.Name);
            callback?.Invoke(false);
            return;
        }
        
        LevelManager.ConsumeLevelPoints(upgradeCost);
        callback?.Invoke(true);
    }
}
}
