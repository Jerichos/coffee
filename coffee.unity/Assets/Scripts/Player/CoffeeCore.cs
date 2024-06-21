using System;
using POLYGONWARE.Coffee.CoffeeGenerators;
using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Coffee.Player
{
public class CoffeeCore : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer _coffeeCore;
    [SerializeField] private SpriteRenderer _gen1;
    
    [SerializeField] private PlayerManager _player;
    
    private void Awake()
    {
        
    }

    private void OnGeneratorBought(CoffeeGenerator generator)
    {
        if (generator.GeneratorSO.ID == "gen_1")
        {
            
        }
    }
    
    private void OnEnable()
    {
        _player.GeneratorManager.GeneratorBoughtEvent += OnGeneratorBought;
    }

    private void OnDisable()
    {
        _player.GeneratorManager.GeneratorBoughtEvent -= OnGeneratorBought;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _player.TapTap();
    }
}
}