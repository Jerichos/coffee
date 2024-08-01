using POLYGONWARE.Coffee.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Coffee.Beans
{
public abstract class Collectible : MonoBehaviour, IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        PlayerManager.LocalPlayer.BeanManager.CollectBean(this);
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogWarning("Collectible object should have a collider component.");
        }
    }
#endif
}
}