using System;
using System.Numerics;
using POLYGONWARE.Coffee.Common;
using POLYGONWARE.Coffee.Theme;
using POLYGONWARE.Coffee.Utilities;
using UnityEngine;

namespace POLYGONWARE.Coffee.Upgrades
{
[CreateAssetMenu(fileName = "new_upgrade", menuName = "data/upgrade", order = 0)]
public class UpgradeSO : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField][TextArea] private string _description;
    [SerializeField] private string _costString;
    [SerializeField] private double _cooldown;
    [SerializeField] private double _value;
    [SerializeField] private ulong _timeToUpgrade;
    [SerializeField] private uint _unlockLevel;
    [SerializeField] private GameObject _prefab;
        
    // class of upgrade
    [SerializeField] private string _typeName;
        
    [Header("Sfx")]
    [SerializeField] private AudioClip _soundClue1;
        
    public string Name => _name;
    public string Description => _description;
    public BigInteger Cost => BigInteger.Parse(_costString);
    public double Cooldown => _cooldown;
    public double Value => _value;
    public ulong TimeToUpgrade => _timeToUpgrade;
    public uint UnlockLevel => _unlockLevel;
    public Sprite Icon => _icon;
    public GameObject Prefab => _prefab;
    public AudioClip SoundClue1 => _soundClue1;
    
    public Type GetUpgradeType()
    {
        Type upgradeType = Type.GetType(_typeName);
        if (upgradeType == null)
        {
            throw new InvalidOperationException($"Type {_typeName} could not be found.");
        }

        return upgradeType;
    }
        
    // Method to create an instance of the upgrade
    public Upgrade CreateUpgradeInstance()
    {
        if (string.IsNullOrEmpty(_typeName))
        {
            throw new InvalidOperationException("Type name is not specified.");
        }
            
        Type upgradeType = Type.GetType(_typeName);
        if (upgradeType == null)
        {
            throw new InvalidOperationException($"Type {_typeName} could not be found.");
        }
            
        if (!typeof(Upgrade).IsAssignableFrom(upgradeType))
        {
            throw new InvalidOperationException($"{_typeName} is not a subclass of Upgrade.");
        }
            
        return (Upgrade)Activator.CreateInstance(upgradeType, new object[]{this});
    }

    public string GetFormattedDescription(Upgrade upgrade)
    {
        // format map
        // [c] cooldown
        // [t] current total value
        // [v] value
        
        if (upgrade != null)
        {
            return upgrade.GetFormattedDescription(); 
        }
        
        string description = _description;
        description = description.Replace("[c]", TextFormatter.ColorText(_cooldown.ToString(), ThemeManager.ColorPalette.NumberPositive));
        description = description.Replace("[t]", "0%");
        description = description.Replace("[v]", TextFormatter.ColorText(_value*100+ "%", ThemeManager.ColorPalette.NumberPositive));
        return description;
    }
}

public struct ReturnMessage
{
    public ReturnCode ReturnCode;
    public string Message;
}
}