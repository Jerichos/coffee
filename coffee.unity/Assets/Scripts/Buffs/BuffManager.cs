using System;
using System.Collections.Generic;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Buffs
{
// manager for buffs
// buff is a temporary effect that can be applied to the player
public class BuffManager
{
    public List<Buff> ActiveBuffs { get; private set; }
    
    public Action<Buff> OnBuffAdded;
    public Action<Buff> OnBuffRemoved;
    
    PlayerManager _playerManager;
    
    public BuffManager(PlayerManager player)
    {
        ActiveBuffs = new();
        _playerManager = player;
    }
    
    public void Update(float deltaTime)
    {
        for (int i = 0; i < ActiveBuffs.Count; i++)
        {
            ActiveBuffs[i].Update(deltaTime);
            
            if (ActiveBuffs[i].TimeLeft <= 0)
            {
                RemoveBuff(ActiveBuffs[i]);
                i--;
            }
        }
    }

    public void AddBuff(BuffSO buffSO)
    {
        AddBuff(new Buff(buffSO));
    }
    
    public void AddBuff(Buff buff)
    {
        if (ActiveBuffs.Contains(buff))
        {
            Debug.LogError("Trying to add a buff that is already active!");
            return;
        }
        
        buff.Apply(_playerManager);
        ActiveBuffs.Add(buff);
        OnBuffAdded?.Invoke(buff);
    }

    public void RemoveBuff(Buff buff)
    {
        if (!ActiveBuffs.Contains(buff))
        {
            Debug.LogError("Trying to remove a buff that is not active!");
            return;
        }
        
        buff.Remove(_playerManager);
        ActiveBuffs.Remove(buff);
        OnBuffRemoved?.Invoke(buff);
    }

    public void RemoveBuff(BuffSO buffSO)
    {
        for (int i = 0; i < ActiveBuffs.Count; i++)
        {
            if (ActiveBuffs[i].BuffSO == buffSO)
            {
                RemoveBuff(ActiveBuffs[i]);
                return;
            }
        }
    }
}
}