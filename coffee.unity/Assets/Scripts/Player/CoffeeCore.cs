using POLYGONWARE.Coffee.CoffeeGenerators;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
public class CoffeeCore : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _coffeeCore;
    [SerializeField] private SpriteRenderer _gen1;
    
    [SerializeField] private PlayerManager _player;
    
    private void Awake()
    {
        _coffeeCore.gameObject.SetActive(false);
        _gen1.gameObject.SetActive(false);
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
}
}