using TMPro;
using UnityEngine;

namespace POLYGONWARE.Coffee.UI
{
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Transform _panel;
    [SerializeField] private RectTransform _fill;
    [SerializeField] private TMP_Text _textValue;

    public float Value
    {
        set => _fill.localScale = new Vector3(Mathf.Clamp(value, 0, 1), 1, 1);
    }

    public void SetValue(float value)
    {
        Value = value;
    }
    
    public void SetText(string text)
    {
        _textValue.SetText(text);
    }
    
    #if UNITY_EDITOR
    [SerializeField][Range(0,1)] private float _value;
    private void OnValidate()
    {
        Value = _value;
    }
#endif
}
}
