using POLYGONWARE.Coffee.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Coffee.Buffs
{
public class FloatingRandomBuff : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BuffSO _buffSO;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerManager.LocalPlayer.BuffManager.AddBuff(_buffSO);
        gameObject.SetActive(false);
    }
}
}