using POLYGONWARE.Coffee.CoffeeGenerators;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Buffs
{
public class Buff
{
    public BuffSO BuffSO { get; private set; }
    
    public double TimeLeft { get; private set; }
    public double V1;

    public BuffTarget TargetAttribute;
    public CoffeeGenerator TargetGenerator;
    
    public Buff(BuffSO buffSO)
    {
        BuffSO = buffSO;
        TimeLeft = buffSO.Duration;
        V1 = buffSO.Value;
    }
    
    public Buff(BuffSO buffSO, double timeLeft, double v1, BuffTarget targetAttribute)
    {
        BuffSO = buffSO;
        TimeLeft = timeLeft;
        V1 = v1;
        TargetAttribute = targetAttribute;
    }
    
    public Buff(BuffSO buffSO, double timeLeft, double v1, BuffTarget targetAttribute, CoffeeGenerator targetGenerator)
    {
        BuffSO = buffSO;
        TimeLeft = timeLeft;
        V1 = v1;
        TargetAttribute = targetAttribute;
        TargetGenerator = targetGenerator;
    }

    public void Update(double deltaTime)
    {
        TimeLeft -= deltaTime;

        if (TimeLeft <= 0)
        {
            // remove buff
        }
    }

    public void Apply(PlayerManager player)
    {
        if(TargetGenerator != null)
        {
            ApplyToGenerator(TargetGenerator);
        }
        else
        {
            ApplyToPlayer(player);
        }
        
        player.RecalculateCps();
    }

    private void ApplyToGenerator(CoffeeGenerator targetGenerator)
    {
        switch (BuffSO.AdditionType)
        {
            case AdditionType.ADDITION:
                targetGenerator.CpsAdditionBonus += BuffSO.Value;
                break;
            case AdditionType.MULTIPLIER:
                targetGenerator.CpsMultiplierBonus += BuffSO.Value;
                break;
        }
    }

    private void ApplyToPlayer(PlayerManager player)
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