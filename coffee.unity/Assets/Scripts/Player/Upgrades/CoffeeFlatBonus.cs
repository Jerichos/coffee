namespace POLYGONWARE.Coffee.Player.Upgrades
{

// permanently adds a flat bonus to the coffee production
// 10 coffees per rank
public class CoffeeFlatBonusUpgrade : Upgrade
{
    public CoffeeFlatBonusUpgrade(UpgradeSO upgradeSO) : base(upgradeSO)
    {
    }

    protected override void OnUpdate(double deltaTime)
    {
        
    }

    protected override void OnApplyUpgrade(PlayerManager player)
    {
        player.CpsAdditionBonus += 10 * Rank;
        player.RecalculateCps();
    }

    protected override void OnRemoveUpgrade(PlayerManager player)
    {
        player.CpsAdditionBonus -= 10 * Rank;
        player.RecalculateCps();
    }
}
}