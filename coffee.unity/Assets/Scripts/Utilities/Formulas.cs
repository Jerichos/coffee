using System.Numerics;
using POLYGONWARE.Coffee.Upgrades;

namespace POLYGONWARE.Coffee.Utilities
{
public static class Formulas
{
    public static BigInteger CalculateUpgradeCost(UpgradeSO upgrade, uint rank)
    {
        return upgrade.Cost * (rank + 1);
    }
    
    public static ulong CalculateUpgradeTime(UpgradeSO upgrade, uint rank)
    {
        return upgrade.TimeToUpgrade * (rank + 1);
    }
}
}