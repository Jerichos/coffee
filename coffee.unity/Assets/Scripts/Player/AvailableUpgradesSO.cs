using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
[CreateAssetMenu(fileName = "New_Available_Upgrades", menuName = "data/Available Upgrades", order = 0)]
public class AvailableUpgradesSO : ScriptableObject
{
    [SerializeField] private UpgradeSO[] _upgrades;

    public UpgradeSO[] Upgrades => _upgrades;
}
}