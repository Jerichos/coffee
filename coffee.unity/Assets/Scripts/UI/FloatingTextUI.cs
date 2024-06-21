using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace POLYGONWARE.Coffee.UI
{
public class FloatingTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Animator _animator;
    
    // TODO: create pool system
    static List<FloatingTextUI> _pool = new();

    private void Awake()
    {
        _pool.Add(this);
        PreWarm(10);
    }

    public FloatingTextUI Create(Transform parent, string text)
    {
        return Create(parent, text, Color.white);
    }
    
    public FloatingTextUI Create(Transform parent, string text, Color textColor)
    {
        var floatingText = GetFromPoolOrCreate(parent);
        floatingText._text.text = text;
        floatingText._text.color = textColor;
        return floatingText;
    }
    
    private void PreWarm(int count)
    {
        while(_pool.Count < count)
        {
            var floatingText = Instantiate(this, transform);
            floatingText.gameObject.SetActive(false);
            _pool.Add(floatingText);
        }
    }
    
    private FloatingTextUI GetFromPoolOrCreate(Transform parent)
    {
        if (_pool.Count > 0)
        {
            var floatingText = _pool[0];
            _pool.RemoveAt(0);
            floatingText.transform.SetParent(parent);
            floatingText.gameObject.SetActive(true);
            return floatingText;
        }
        return Create(parent, "");
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
        _pool.Add(this);
    }
}
}