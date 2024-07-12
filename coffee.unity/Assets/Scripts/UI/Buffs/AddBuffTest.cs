using System;
using POLYGONWARE.Coffee.Buffs;
using POLYGONWARE.Coffee.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI
{
public class AddBuffTest : MonoBehaviour
{
    [SerializeField] private BuffSO _buffSO;
    [SerializeField] private PlayerManager _player;

    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(AddBuff);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(AddBuff);
    }

    private void AddBuff()
    {
        _player.BuffManager.AddBuff(_buffSO);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        gameObject.name = _buffSO.name;
        _button.GetComponentInChildren<TMP_Text>().SetText(_buffSO.name);
    }
#endif
}
}