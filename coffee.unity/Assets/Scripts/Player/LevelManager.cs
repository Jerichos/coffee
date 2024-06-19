using System;
using System.Numerics;
using POLYGONWARE.Coffee.Settings;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
public class LevelManager
{
    public uint Level { get; private set; }
    public uint LevelPoints { get; private set; }

    public BigInteger CoffeesToNextLevel => (BigInteger)(GameSettings.BASE_COFFEES_TO_LEVEL * Math.Pow(GameSettings.LEVEL_DIFFICULTY_MULTIPLIER, Level - 1));
    //public BigInteger CoffeesToPreviousLevel => (BigInteger)(GameSettings.BASE_COFFEES_TO_LEVEL * Math.Pow(GameSettings.LEVEL_DIFFICULTY_MULTIPLIER, Level - 2));

    private bool _canLevelUp;
    public bool CanLevelUp
    {
        get => _canLevelUp;
        private set
        {
            _canLevelUp = value;
            CanLevelUPEvent?.Invoke(value);
        }
    }
    
    public Action<bool> CanLevelUPEvent;
    public Action<uint> LevelChangedEvent;
    public Action<uint> LevelPointsChangedEvent;

    private readonly PlayerManager _player;

    public LevelManager(PlayerManager player)
    {
        Level = 1;
        
        _player = player;
        _player.OnTotalCoffeesGeneratedChanged += OnTotalCoffeesGeneratedChanged;
        
        OnTotalCoffeesGeneratedChanged(_player.TotalCoffeesGenerated);
    }

    public void LevelUp()
    {
        Level++;
        LevelPoints++;
        
        
        // TODO: Add some kind of reward for leveling up
        
        OnTotalCoffeesGeneratedChanged(_player.TotalCoffeesGenerated);
        LevelPointsChangedEvent?.Invoke(LevelPoints);
        LevelChangedEvent?.Invoke(Level);
    }
    
    public BigInteger GetCoffeesForPreviousLevel()
    {
        if (Level == 1)
            return 0;
        
        return (BigInteger)(GameSettings.BASE_COFFEES_TO_LEVEL * Math.Pow(GameSettings.LEVEL_DIFFICULTY_MULTIPLIER, Level - 2));
    }

    private void OnTotalCoffeesGeneratedChanged(BigInteger amount)
    {
        if (amount >= CoffeesToNextLevel)
        {
            CanLevelUp = true;
        }
        else
        {
            CanLevelUp = false;
        }
    }

    public void ConsumeLevelPoints(uint points)
    {
        if (LevelPoints <= 0)
        {
            Debug.Log("Not enough level points to consume. Required: " + points + ", available: " + LevelPoints);
            return;
        }
        
        LevelPoints -= points;
        LevelPointsChangedEvent?.Invoke(LevelPoints);
        Debug.Log("Level points: " + LevelPoints);
    }
}
}