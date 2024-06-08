using System;

namespace POLYGONWARE.Coffee.Player
{
public class PrestigeManager
{
    public uint PrestigeLevel;
    public uint PrestigePoints;
    
    public uint CoffeesToNextPrestigeLevel => (1 + PrestigeLevel) * 100;
    public uint CoffeesToPreviousPrestigeLevel => PrestigeLevel * 100;

    public Action<uint> OnPrestigeLevelChanged;
    public Action<uint> OnPrestigePointsChanged;
    public Action<uint> OnCoffeesToNextPrestigeLevelChanged;
    public Action<uint> OnCoffeesToPreviousPrestigeLevelChanged;
}
}