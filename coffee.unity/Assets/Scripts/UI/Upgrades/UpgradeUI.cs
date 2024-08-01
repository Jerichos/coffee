using System;
using POLYGONWARE.Coffee.Common.UI;
using POLYGONWARE.Coffee.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI.Upgrades
{
public class UpgradeUI : SelectableBase
{
    [Header("UpgradeUI")]
    [SerializeField] private UpgradeSO _upgradeSO;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _timerFill;
    [SerializeField] private Image _finishedFill;
    
    private CanvasGroup _canvasGroup;

    private bool _pending;
    private bool _finished;
    
    public UpgradeSO UpgradeSO => _upgradeSO;
    private UpgradeStateData _upgradeStateData;
    
    private static readonly int FinishedBoolHash = Animator.StringToHash("finished");
    
    private void OnDisable()
    {
        CancelInvoke();
    }

    public void SetState(UpgradeStateData upgradeStateData)
    {
        _upgradeStateData = upgradeStateData;
        CancelInvoke();
        switch (upgradeStateData.State)
        {
            case UpgradeState.INACTIVE:
                _icon.color = Color.black;
                break;
            case UpgradeState.ACTIVE:
                _icon.color = Color.white;
                _timerFill.fillAmount = 0;
                Animator.SetBool(FinishedBoolHash, false);
                break;
            case UpgradeState.PENDING:
                InvokeRepeating(nameof(UpdateTimer), 0, 0.03f);
                _timerFill.color = Color.red;
                break;
            case UpgradeState.FINISHED:
                Animator.SetBool(FinishedBoolHash, true);
                _timerFill.color = Color.green;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(upgradeStateData.State), upgradeStateData.State, null);
        }
    }
    
    private void UpdateTimer()
    {
        float progress = (float) (_upgradeStateData.EndTime - DateTime.Now).TotalSeconds / (float) (_upgradeStateData.EndTime - _upgradeStateData.StartTime).TotalSeconds;
        _timerFill.fillAmount = 1 - progress;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(_upgradeSO == null) 
            return;
        
        _icon.sprite = _upgradeSO.Icon;
        name = _upgradeSO.name + " Upgrade";
    }
#endif
}
}