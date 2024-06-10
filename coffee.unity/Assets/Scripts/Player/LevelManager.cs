using System;

namespace POLYGONWARE.Coffee.Player
{
public class LevelManager
{
    public uint Level { get; private set; }
    public uint LevelPoints { get; private set; }
    
    public uint CookiesToNextLevel => 10 + (1+Level) * 2^(1+Level);
    public uint CookiesToPreviousLevel => Level * 2^Level;

    private bool _canLevelUp;
    public bool CanLevelUpgit
    {
        get => _canLevelUp;
        private set
        {
            _canLevelUp = value;
            CanLevelUPEvent?.Invoke(value);
        }
    }
    
    public Action<bool> CanLevelUPEvent;

    private PlayerManager _player;
    
    public LevelManager(PlayerManager player)
    {
        Level = 0;
        
        _player = player;
        _player.OnTotalCoffeesGeneratedChanged += OnTotalCoffeesGeneratedChanged;
        OnTotalCoffeesGeneratedChanged(_player.TotalCoffeesGenerated);
    }

    private void OnTotalCoffeesGeneratedChanged(uint amount)
    {
        if (amount >= CookiesToNextLevel)
        {
            Level++;
            LevelPoints++;
            _player.OnLevelChanged?.Invoke(Level);
            _player.OnLevelPointsLeftChanged?.Invoke(LevelPoints);
        }
    }
}
}