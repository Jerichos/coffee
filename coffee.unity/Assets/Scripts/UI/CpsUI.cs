using System;
using POLYGONWARE.Coffee.Theme;
using TMPro;
using UnityEngine;

namespace POLYGONWARE.Coffee.UI
{
public class CpsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cpsText;
    [SerializeField] private FloatingTextUI _floatingTextPrefab;

    private void Awake()
    {
        _floatingTextPrefab.gameObject.SetActive(false);
    }

    public void SetValue(double oldValue, double newValue)
    {
        _cpsText.text = newValue.ToString("F1");
        
        var diff = newValue - oldValue;
        
        if (diff == 0)
            return;

        Color color;
        string text;
        if (diff > 0)
        {
            text = $"+{diff:F2}";
            color = ThemeManager.ColorPalette.NumberPositive;
        }
        else
        {
            text = $"{diff:F2}";
            color = ThemeManager.ColorPalette.NumberNegative;
        }
        
        var floatingText = _floatingTextPrefab.Create(transform, text, color);
    }
}
}