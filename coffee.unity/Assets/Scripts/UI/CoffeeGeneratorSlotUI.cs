using System;
using System.Numerics;
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
    [SerializeField] private TMP_Text _cpsPerCostText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _upgradeButton;

    
    [SerializeField] private Color _canBuyColor;
    [SerializeField] private Color _cantBuyColor;
    
    private PlayerManager _player;
    private CoffeeGeneratorSO _generatorSO;
    
    public CoffeeGeneratorSO GeneratorSO => _generatorSO;

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

    private void OnCoffeesChanged(BigInteger amount)
    {
        if (_buyButton == null)
        {
            Debug.LogWarning("Buy button is null", gameObject);
            return;
        }
        // disable buy button if not enough coffees
        if (_player.GeneratorManager.GetCost(_generatorSO) > _player.Coffees)
        {
            _buyButton.interactable = false;
            // _buyButton.GetComponent<Image>().color = _cantBuyColor;
            _costText.color = _cantBuyColor;
        }
        else
        {
            _buyButton.interactable = true;
            _costText.color = _canBuyColor;
            // _buyButton.GetComponent<Image>().color = _canBuyColor;
        }
    }

    public void SubscribeToBuyButton(Action<CoffeeGeneratorSO> buyAction)
    {
        _buyButton.onClick.AddListener(() => buyAction?.Invoke(_generatorSO));
    }
    
    public void SubscribeToUpgradeButton(Action<CoffeeGeneratorSlotUI> upgradeAction)
    {
        _upgradeButton.onClick.AddListener(() => upgradeAction?.Invoke(this));
    }

    private void OnDestroy()
    {
        _buyButton.onClick.RemoveAllListeners();
    }

    public void UpdateSlot()
    {
        OnCoffeesChanged(_player.Coffees);
        //uint newPrice = _player.GeneratorManager.GetUpgradeCost(_generatorSO);
        //Debug.Log(_generatorSO.Name + " newPrice: " + newPrice);
        
        _costText.SetText(_player.GeneratorManager.GetCost(_generatorSO).ToString());
        _amountText.SetText(_player.GeneratorManager.GetGeneratorAmount(_generatorSO).ToString());
        _cpsPerCostText.SetText(_player.GeneratorManager.GetCpsPerCost(_generatorSO).ToString("F0"));

        if (CanUpgrade())
        {
            _upgradeButton.gameObject.SetActive(true);
            _upgradeButton.GetComponentInChildren<TMP_Text>().SetText("+1% for " + _player.GeneratorManager.GetUpgradeCost(_generatorSO) + "LP");
        }
        else
        {
            _upgradeButton.gameObject.SetActive(false);
        }
    }
  
    public bool CanUpgrade() 
    {
        if (!_player.GeneratorManager.Generators.TryGetValue(_generatorSO, out var generator))
        {
            return false;
        }
        
        return _player.LevelManager.LevelPoints >= generator.LevelUpgradeCost;
    }

    private void OnUpgradeCallback(bool upgraded)
    {
        if (upgraded)
        {
            UpdateSlot();
        }
    }
}
}