using UnityEngine;

namespace POLYGONWARE.Coffee.Buffs
{

public enum AdditionType
{
    ADDITION,
    MULTIPLIER
}

public enum BuffTarget
{
    TAP,
    CPS,
    COST,
}

[CreateAssetMenu(fileName = "NewBuff", menuName = "data/Buff", order = 0)]
public class BuffSO : ScriptableObject
{
    public Sprite Icon;
    
    public string Name;
    public string Description;
    
    public float Duration;
    
    public double Value;
    
    public AdditionType AdditionType;
}
}