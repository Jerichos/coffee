using System;
using System.Collections.Generic;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Beans
{

/// <summary>
/// Player can unlock Bean Research Facility to unlock new beans.
/// Player can research new beans which would then spawn in the game world.
/// Researching new beans takes time and resources.
/// </summary>
public class BeanResearchFacility : MonoBehaviour
{
    [SerializeField] private PlayerManager _player;
    [SerializeField] private SupportedBeansSO _supportedBeans;

    public SupportedBeansSO SupportedBeans => _supportedBeans;

    public Dictionary<BeanSO, uint> ActiveBeans { get; private set; } = new(); // unlocked bean and rank
    public Action<Dictionary<BeanSO, uint>> UnlockedBeansChangedEvent;

    public Dictionary<BeanSO, UnlockingData> UnlockingBeans { get; private set; } = new();
    public Action<Dictionary<BeanSO, UnlockingData>> UnlockingBeansChangedEvent;

    public Action<BeanSO> ResearchFinishedEvent;
    public Action<BeanSO> ResearchStartedEvent;
    
    public void StartResearch(BeanSO beanSO)
    {
        if (ActiveBeans.ContainsKey(beanSO))
        {
            Debug.Log("Bean is already unlocked. Increase rank instead.");
            ActiveBeans[beanSO]++;
            
            UnlockedBeansChangedEvent?.Invoke(ActiveBeans);
            return;
        }

        if (UnlockingBeans.ContainsKey(beanSO)) // if bean is being unlocked
        {
            Debug.LogError("Bean is already being unlocked.");
            return;
        }

        if (!CanUnlock(beanSO))
        {
            Debug.LogError("Requirements not met to unlock bean.");
            return;
        }
        
        UnlockingBeans.Add(beanSO, new UnlockingData { StartTime = DateTime.Now });
        UnlockingBeansChangedEvent?.Invoke(UnlockingBeans);
    }

    private bool CanUnlock(BeanSO beanSO)
    {
        throw new NotImplementedException();
    }
}

public struct UnlockingData
{
    public DateTime StartTime;
    public TimeSpan RemainingTime => StartTime - DateTime.Now;
    public bool Finished => RemainingTime.TotalSeconds <= 0;
}

}