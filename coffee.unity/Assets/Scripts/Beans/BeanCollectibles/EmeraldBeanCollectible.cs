using System;
using POLYGONWARE.Coffee.Player;

namespace POLYGONWARE.Coffee.Beans.BeanCollectibles
{

// Reduces cooldown of skills and abilities by [v1%].
public class EmeraldBeanCollectible : BeanCollectible
{
    protected override void OnCollect(PlayerManager player, ResearchedBeanData beanData)
    {
        double cooldownReduction = BeanSO.V1 * beanData.Rank;
        throw new NotImplementedException();
    }
}
}