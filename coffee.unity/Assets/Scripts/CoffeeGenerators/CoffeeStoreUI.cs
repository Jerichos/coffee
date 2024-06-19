using System;
using System.Collections.Generic;
using POLYGONWARE.Coffee.Player;
using POLYGONWARE.Coffee.UI;
using UnityEngine;

namespace POLYGONWARE.Coffee.CoffeeGenerators
{

public class CoffeeStoreUI : MonoBehaviour
{
    [SerializeField] private Transform _generatorSlotsParent;
    [Space]
    [SerializeField] private PlayerManager _player;
    [SerializeField] private CoffeeGeneratorTable _availableGenerators;
    [SerializeField] private CoffeeGeneratorSlotUI _generatorSlotPrefab;
    
    private readonly List<CoffeeGeneratorSlotUI> _generatorSlots = new();

    private void Awake()
    {
        if(_generatorSlotPrefab)
            _generatorSlotPrefab.gameObject.SetActive(false);
    }

    private void Start()
    {
        _player.GeneratorManager.GeneratorsChangedEvent += OnGeneratorsChanged;
        _player.LevelManager.LevelPointsChangedEvent += OnLevelPointsChanged;
        
        OnGeneratorsChanged(_player.GeneratorManager.Generators);
        OnLevelPointsChanged(_player.LevelManager.LevelPoints);
        
        foreach (var generator in _availableGenerators.Generators)
        {
            var slot = Instantiate(_generatorSlotPrefab, transform);
            slot.transform.SetParent(_generatorSlotsParent);
            slot.Initialize(generator, _player);
            slot.SubscribeToBuyButton(BuyGenerator);
            slot.SubscribeToUpgradeButton(UpgradeGenerator);
            slot.gameObject.SetActive(true);
            _generatorSlots.Add(slot);
        }
    }

    private void UpgradeGenerator(CoffeeGeneratorSlotUI generatorSlotUI)
    {
        if(_player.LevelManager.LevelPoints <= 0)
            return;
        
        _player.UpgradeGenerator(generatorSlotUI.GeneratorSO, (upgraded =>
        {
            if(upgraded)
                generatorSlotUI.UpdateSlot();
        }));
    }

    private void OnLevelPointsChanged(uint levelPoints)
    {
        foreach (var slot in _generatorSlots)
        {
            slot.UpdateSlot();
        }
    }

    private void OnGeneratorsChanged(Dictionary<CoffeeGeneratorSO, CoffeeGenerator> generators)
    {
        // update slots
        Debug.Log("generators changed, update slots");
        foreach (var slot in _generatorSlots)
        {
            slot.UpdateSlot();
        }
    }

    private void BuyGenerator(CoffeeGeneratorSO generator)
    {
        var cost = _player.GeneratorManager.GetCost(generator);
        if (cost > _player.Coffees)
        {
            Debug.Log("not enough coffees to buy generator");
            return;
        }
        
        _player.ConsumeCoffee(cost);
        _player.GeneratorManager.AddGenerator(generator);
    }
}
}