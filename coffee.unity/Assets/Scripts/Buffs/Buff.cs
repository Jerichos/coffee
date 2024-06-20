using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Buffs
{
public class Buff
{
    public BuffSO BuffSO { get; private set; }
    
    public float TimeLeft { get; private set; }
    
    public Buff(BuffSO buffSO)
    {
        BuffSO = buffSO;
        TimeLeft = buffSO.Duration;
    }

    public void Update(float deltaTime)
    {
        TimeLeft -= deltaTime;

        if (TimeLeft <= 0)
        {
            // remove buff
        }
    }

    public void Apply(PlayerManager player)
    {
        switch (BuffSO.AdditionType)
        {
            case AdditionType.ADDITION:
                player.CpsAdditionBonus += BuffSO.Value;
                Debug.Log($"applying addition buff {BuffSO.Value}, total addition: {player.CpsAdditionBonus}");
                break;
            case AdditionType.MULTIPLIER:
                player.CpsMultiplierBonus += BuffSO.Value;
                Debug.Log($"applying multiplier buff {BuffSO.Value}, total multiplier: {player.CpsMultiplierBonus}");
                break;
        }
        
        player.RecalculateCps();
    }
    
    public void Remove(PlayerManager player)
    {
        switch (BuffSO.AdditionType)
        {
            case AdditionType.ADDITION:
                player.CpsAdditionBonus -= BuffSO.Value;
                break;
            case AdditionType.MULTIPLIER:
                player.CpsMultiplierBonus -= BuffSO.Value;
                break;
        }
        player.RecalculateCps();
    }
}
}