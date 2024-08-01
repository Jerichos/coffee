using System.Numerics;
using POLYGONWARE.Coffee.Player;

namespace POLYGONWARE.Coffee.Beans.BeanCollectibles
{

// Instantly grants a massive coffee boost, multiplying the current coffee count by [v1].
// TODO: Maybe change the name to Hoarder Bean?
public class MultiplierBeanCollectible : BeanCollectible
{
    protected override void OnCollect(PlayerManager player, ResearchedBeanData beanData)
    {
        BigInteger cps = (BigInteger)((BeanSO.V1 * beanData.Rank) * (double)player.Coffees);
        player.AddGeneratedCoffee(cps);
    }
}
}