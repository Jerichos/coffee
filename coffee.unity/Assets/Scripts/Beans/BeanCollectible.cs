using POLYGONWARE.Coffee.Game;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.Beans
{

/// <summary>
/// Beans are the main collectible in the game.
/// Special beans can be unlocked by completing certain tasks and challenges.
/// Special beans once unlocked can spawn in the game world randomly or by specific conditions.
/// Special beans when collected grants the player bonuses.
/// </summary>
public abstract class BeanCollectible : Collectible
{
    [SerializeField] private BeanSO _beanSO;
    public BeanSO BeanSO => _beanSO;
    
    [Header("Audio")]
    [SerializeField] private AudioClip _collectSound;

    public void Collect(PlayerManager player, ResearchedBeanData beanData)
    {
        AudioManager.PlaySfx(_collectSound);
        
        if(player)
            OnCollect(player, beanData);
    }
    
    protected abstract void OnCollect(PlayerManager player, ResearchedBeanData beanData);
}
}