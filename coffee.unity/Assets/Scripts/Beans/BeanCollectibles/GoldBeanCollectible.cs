using System.Numerics;
using POLYGONWARE.Coffee.Player;

namespace POLYGONWARE.Coffee.Beans.BeanCollectibles
{

// Grants [v1t] worth of cps beans.
public class GoldBeanCollectible : BeanCollectible
{
    protected override void OnCollect(PlayerManager player, ResearchedBeanData beanData)
    {
        BigInteger cps = (BigInteger)(player.Cps * BeanSO.V1 * beanData.Rank);
        player.AddGeneratedCoffee(cps);
    }
}
}