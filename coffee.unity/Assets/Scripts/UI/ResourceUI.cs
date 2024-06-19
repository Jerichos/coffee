using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Coffee.UI
{
public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceText;
    [SerializeField] private Image _resourceIcon;
    
    public void SetText(string text)
    {
        _resourceText.text = text;
    }

    public void SetIcon(Sprite icon)
    {
        _resourceIcon.sprite = icon;
    }
}
}