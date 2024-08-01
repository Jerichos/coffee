using System;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Beans.BeanCollectibles
{

// Grants [v1%cps] beans.
// grants 1 minute worth of cps.
// Temporarily opens the Rift, a mysterious dimension where special dark beans spawn. These dark beans provide various boosts and bonuses when collected.
public class ShadowBeanCollectible : BeanCollectible
{
    [SerializeField] private GameObject BeanRiftPrefab;
    
    protected override void OnCollect(PlayerManager player, ResearchedBeanData beanData)
    {
        // spawn bean rift at the bean location
        GameObject beanRift = Instantiate(BeanRiftPrefab, transform.position, Quaternion.identity);
    }
}
}