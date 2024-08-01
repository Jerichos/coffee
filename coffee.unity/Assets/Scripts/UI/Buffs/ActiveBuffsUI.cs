using System.Collections.Generic;
using POLYGONWARE.Coffee.Buffs;
using POLYGONWARE.Coffee.Player;
using UnityEngine;

namespace POLYGONWARE.Coffee.UI.Buffs
{
public class ActiveBuffsUI : MonoBehaviour
{
    [SerializeField] private PlayerManager _player;
    [SerializeField] private BuffUI _buffUIPrefab;
    [SerializeField] private GameObject _panel;
    
    private readonly Dictionary<Buff, BuffUI> _buffUIs = new();

    private void Awake()
    {
        _buffUIPrefab.gameObject.SetActive(false);
        _panel.SetActive(false);
    }

    private void OnEnable()
    {
        _player.BuffManager.OnBuffAdded += OnBuffAdded;
        _player.BuffManager.OnBuffRemoved += OnBuffRemoved;
        
        foreach (var buff in _player.BuffManager.ActiveBuffs)
        {
            OnBuffAdded(buff);
        }
    }
    
    private void OnDisable()
    {
        _player.BuffManager.OnBuffAdded -= OnBuffAdded;
        _player.BuffManager.OnBuffRemoved -= OnBuffRemoved;
    }

    private void OnBuffRemoved(Buff buff)
    {
        if (_buffUIs.TryGetValue(buff, out var buffUI))
        {
            Destroy(buffUI.gameObject);
            _buffUIs.Remove(buff);
        }
        
        // close view if no more buffs
        if (_buffUIs.Count == 0)
            _panel.SetActive(false);
        
        Debug.LogWarning("Buff removed!");
    }

    private void OnBuffAdded(Buff buff)
    {
        // check first if the buff is already added
        if (_buffUIs.ContainsKey(buff))
        {
            Debug.LogError("Trying to add a buff that is already active!");
            return;
        }
        
        var buffUI = Instantiate(_buffUIPrefab, _panel.transform);
        buffUI.Initialize(buff);
        buffUI.gameObject.SetActive(true);
        _buffUIs.Add(buff, buffUI);
        
        // open view if there are buffs
        if (_buffUIs.Count == 1)
            _panel.SetActive(true);
        
        Debug.LogWarning("Buff added!");
    }
}
}