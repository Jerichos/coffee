using System;
using System.Collections.Generic;
using System.Numerics;
using POLYGONWARE.Coffee.Buffs;
using POLYGONWARE.Coffee.CoffeeGenerators;
using POLYGONWARE.Coffee.Game;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
[DefaultExecutionOrder(-10)]
public class PlayerManager : MonoBehaviour
{
    // TODO: move to resources
    public BigInteger Coffees { get; private set; }
    // TODO: move to stats
    public BigInteger TotalCoffeesGenerated { get; private set; }
    
    public double Cps { get; private set; }
    
    public LevelManager LevelManager { get; private set; }
    public PrestigeManager PrestigeManager { get; private set; }
    public GeneratorManager GeneratorManager { get; private set; }
    public BuffManager BuffManager { get; private set; }
    public UpgradeManager UpgradeManager { get; private set; }
    public BeanManager BeanManager { get; private set; }
    public AbilityManager AbilitiesManager { get; set; }
    
    public Action<BigInteger> OnCoffeesChanged;
    public Action<BigInteger> OnTotalCoffeesGeneratedChanged;
    public Action<BigInteger> TapTapEvent;
    public Action<double, double> OnCpsChanged;
    public Action<PlayerStats> PlayerStatsChangedEvent;

    public PlayerStats PlayerStats;

    private double _cookiesTemp;
    
    public double CpsMultiplierBonus { get; set; }
    public double CpsAdditionBonus { get; set; }

    public static PlayerManager LocalPlayer;

    private void Awake()
    {
        Debug.Log("PlayerManager Awake");
        
        if (LocalPlayer == null)
            LocalPlayer = this;
        
        CpsMultiplierBonus = 1;
        
        LevelManager = new LevelManager(this);
        PrestigeManager = new PrestigeManager();
        GeneratorManager = new GeneratorManager();
        BuffManager = new BuffManager(this);
        UpgradeManager = new UpgradeManager(this);
        BeanManager = new BeanManager(this);
    }

    private void Update()
    {
        double deltaTime = Time.deltaTime * GameManager.Instance.FastForward;
        
        BuffManager.Update(deltaTime);
        UpgradeManager.Update(deltaTime);
        
        _cookiesTemp += Cps * deltaTime;
        
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
        RecalculateCps();
    }
    
    public void RecalculateCps()
    {
        var prevCps = Cps;
        Cps = GeneratorManager.GetTotalCps();
        Cps += CpsAdditionBonus;
        Cps *= CpsMultiplierBonus;
        
        OnCpsChanged?.Invoke(prevCps, Cps);
    }

    private void OnDisable()
    {
        CancelInvoke();
        GeneratorManager.GeneratorsChangedEvent -= OnGeneratorsChanged;
    }

    public void TapTap()
    {
        BigInteger tapTapCoffees = 1;
        AddGeneratedCoffee(tapTapCoffees);
        PlayerStats.TapTapCount++;
        TapTapEvent?.Invoke(tapTapCoffees);
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
    
    public void AddGeneratedCoffee(BigInteger amount)
    {
        Coffees += amount;
        TotalCoffeesGenerated += amount;
        
        OnCoffeesChanged?.Invoke(Coffees);
        OnTotalCoffeesGeneratedChanged?.Invoke(TotalCoffeesGenerated);
    }
    
    public void AddCoffeeBonus(BigInteger bonus)
    {
        AddGeneratedCoffee(bonus);
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
