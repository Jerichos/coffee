using System;
using POLYGONWARE.Coffee.Buffs;
using POLYGONWARE.Coffee.Player;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI
{
public class RemoveBuffTest : MonoBehaviour
{
    [SerializeField] private BuffSO _buffSO;
    [SerializeField] private PlayerManager _player;

    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(RemoveBuff);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(RemoveBuff);
    }

    private void RemoveBuff()
    {
        _player.BuffManager.RemoveBuff(_buffSO);
    }
}
}