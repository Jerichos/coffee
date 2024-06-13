using System;
using POLYGONWARE.Coffee.CoffeeGenerators;
using POLYGONWARE.Coffee.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI
{
public class CoffeeGeneratorSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Button _buyButton;

    private PlayerManager _player;
    private CoffeeGeneratorSO _generatorSO;

    public void Initialize(CoffeeGeneratorSO generator, PlayerManager player)
    {
        _player = player;
        _generatorSO = generator;
        
        _iconImage.sprite = generator.Icon;
        _nameText.text = generator.Name;
        // _descriptionText.text = generator.Description;
        
        _player.OnCoffeesChanged += OnCoffeesChanged;
        UpdateSlot();
    }

    private void OnCoffeesChanged(uint amount)
    {
        if (_buyButton == null)
        {
            Debug.LogWarning("Buy button is null", gameObject);
            return;
        }
        // disable buy button if not enough coffees
        if(_player.GeneratorManager.GetGeneratorCost(_generatorSO) > _player.Coffees)
            _buyButton.interactable = false;
        else
            _buyButton.interactable = true;
    }

    public void SubscribeToBuyButton(Action<CoffeeGeneratorSO> buyAction)
    {
        _buyButton.onClick.AddListener(() => buyAction?.Invoke(_generatorSO));
    }

    private void OnDestroy()
    {
        _buyButton.onClick.RemoveAllListeners();
    }

    public void UpdateSlot()
    {
        OnCoffeesChanged(_player.Coffees);
        uint newPrice = _player.GeneratorManager.GetGeneratorCost(_generatorSO);
        Debug.Log(_generatorSO.Name + " newPrice: " + newPrice);
        
        _costText.SetText(_player.GeneratorManager.GetGeneratorCost(_generatorSO).ToString());
        _amountText.SetText(_player.GeneratorManager.GetGeneratorAmount(_generatorSO).ToString());
    }
}
}