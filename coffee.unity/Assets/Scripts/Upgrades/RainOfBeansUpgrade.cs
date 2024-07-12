using POLYGONWARE.Coffee.Game;
using POLYGONWARE.Coffee.Player;
using UnityEngine;
using Random = System.Random;

namespace POLYGONWARE.Coffee.Upgrades
{
// There is % change that a n beans will fall from the sky. Each caught bean gives you coffee (%cps).
// % means all the tim is raining
// so we must calculate depending on raintime the probability of rain start
public class RainOfBeansUpgrade : Upgrade
{
    public double Chance { get; private set; }
    public double Value { get; private set; }
    public uint Beans { get; private set; }
    public double RainTime { get; private set; } = 5;
    
    private bool _isRainActive;
    private int _beenCount;
    
    private double _t;
    
    private static readonly Random Random = new Random();
    
    
    public RainOfBeansUpgrade(UpgradeSO upgradeSO) : base(upgradeSO)
    {
    }

    protected override void OnUpdate(double deltaTime)
    {
        // Probability of raining at any given second
        double probabilityPerSecond = Chance / 100.0;

        // Check if it is currently raining
        if (_isRainActive)
        {
            _t += deltaTime;
            
            // Spawn beans over time
            
            var spawnTime = RainTime / Beans;
            if (_t >= spawnTime)
            {
                _beenCount++;
                _t = 0;
                var bean = SpawnBean();
                Debug.Log("RainOfBeansUpgrade: Bean spawned");
            }
            
            if (_t * _beenCount >= RainTime)
            {
                _isRainActive = false;
                _t = 0;
            }
        }
        else
        {
            // Probability of rain starting this frame
            float probabilityThisFrame = (float)probabilityPerSecond * (float)deltaTime;

            if (Random.NextDouble() < probabilityThisFrame)
            {
                StartRaining();
                _beenCount = 0;
                _isRainActive = true;
                _t = 0;
            }
        }
    }

    private GameObject SpawnBean()
    {
        var newBeam = Object.Instantiate(UpgradeSO.Prefab);
        // move bean on top of the screen, GameArea, but at random width
        var top = GameArea.Instance.Max.y;
        var left = GameArea.Instance.Min.x;
        var right = GameArea.Instance.Max.x;
        newBeam.transform.position = new Vector3(Random.Next((int)left, (int)right), top, 0);
        return newBeam;        
    }

    private void StartRaining()
    {
        if (_isRainActive)
        {
            // TODO: maybe we should reset the timer?
            Debug.Log("RainOfBeansUpgrade: Raining is intensifying!");
            _t = 0;
            return;
        }

        Debug.Log("RainOfBeansUpgrade: Start raining");
    }

    protected override void OnApplyUpgrade(PlayerManager player)
    {
    }

    protected override void OnRemoveUpgrade(PlayerManager player)
    {
    }
}
}