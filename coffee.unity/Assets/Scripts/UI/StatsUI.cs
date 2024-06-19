using System;
using POLYGONWARE.Coffee.Player;
using POLYGONWARE.Coffee.Utilities;
using TMPro;
using UnityEngine;

namespace POLYGONWARE.Coffee.UI
{
public class StatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _statsText;
    [SerializeField] private PlayerManager _player;

    private void OnPlayerStatsChanged(PlayerStats playerPlayerStats)
    {
        _statsText.text = $"TapTapCount: {playerPlayerStats.TapTapCount}\n" +
                          $"TotalGeneratedCoffee: {playerPlayerStats.TotalGeneratedCoffee}\n" +
                          $"TotalSpentCoffee: {playerPlayerStats.TotalSpentCoffee}\n" +
                          $"MaxLevelReached: {playerPlayerStats.MaxLevelReached}\n" +
                          $"SecondsPlayed: {Util.FormatSecondsToHHMM(playerPlayerStats.SecondsPlayed)}";
    }
    
    private void OnEnable()
    {
        _player.PlayerStatsChangedEvent += OnPlayerStatsChanged;
        OnPlayerStatsChanged(_player.PlayerStats);
    }

    private void OnDisable()
    {
        _player.PlayerStatsChangedEvent -= OnPlayerStatsChanged;
    }
}
}