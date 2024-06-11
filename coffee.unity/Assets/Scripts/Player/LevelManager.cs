using System;

namespace POLYGONWARE.Coffee.Player
{
public class LevelManager
{
    public uint Level { get; private set; }
    public uint LevelPoints { get; private set; }
    
    public uint CoffeesToNextLevel => 10 + (1+Level) * 2^(1+Level);
    public uint CookiesToPreviousLevel => Level * 2^Level;

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
    public Action<uint> OnLevelChanged;

    private PlayerManager _player;
    
    public LevelManager(PlayerManager player)
    {
        Level = 0;
        
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
        OnLevelChanged?.Invoke(Level);
    }

    private void OnTotalCoffeesGeneratedChanged(uint amount)
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
}
}