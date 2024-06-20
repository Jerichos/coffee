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
    CLICK,
    TOTAL_CPS,
    TOTAL_COST,
    GEN_1_CPS,
    GEN_2_CPS,
    GEN_3_CPS,
    GEN_4_CPS,
    GEN_5_CPS,
    GEN_6_CPS,
    GEN_7_CPS,
    GEN_8_CPS,
    GEN_9_CPS,
    GEN_10_CPS,
    GEN_11_CPS,
    GEN_12_CPS,
    GEN_13_CPS,
    GEN_14_CPS,
    GEN_15_CPS,
    GEN_16_CPS,
    GEN_17_CPS,
    GEN_18_CPS,
    GEN_19_CPS,
    GEN_20_CPS,
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