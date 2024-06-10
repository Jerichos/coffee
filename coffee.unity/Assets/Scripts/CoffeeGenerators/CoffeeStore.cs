﻿using System;
using System.Collections.Generic;
using POLYGONWARE.Coffee.Player;
using POLYGONWARE.Coffee.UI;
using UI;
using UnityEngine;

namespace POLYGONWARE.Coffee.CoffeeGenerators
{
public class CoffeeStore : MonoBehaviour
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

    private void OnEnable()
    {
        _player.GeneratorManager.GeneratorsChangedEvent += OnGeneratorsChanged;
        OnGeneratorsChanged(_player.GeneratorManager.Generators);
        
        foreach (var generator in _availableGenerators.Generators)
        {
            var slot = Instantiate(_generatorSlotPrefab, transform);
            slot.transform.SetParent(_generatorSlotsParent);
            slot.Initialize(generator, _player);
            slot.SubscribeToBuyButton(BuyGenerator);
            slot.gameObject.SetActive(true);
            _generatorSlots.Add(slot);
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
        var cost = _player.GeneratorManager.GetGeneratorCost(generator);
        if (cost > _player.Coffees)
        {
            Debug.Log("not enough coffees to buy generator");
            return;
        }
        
        _player.ConsumeCoffee(cost);
        _player.GeneratorManager.AddGenerator(generator);
    }

    private void OnDisable()
    {
        foreach (var slot in _generatorSlots)
        {
            Destroy(slot.gameObject);
        }
        _generatorSlots.Clear();
    }
}
}