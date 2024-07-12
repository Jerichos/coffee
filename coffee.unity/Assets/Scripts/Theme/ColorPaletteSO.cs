using UnityEngine;

namespace POLYGONWARE.Coffee.Theme
{
[CreateAssetMenu(fileName = "New_Color_Palette", menuName = "theme/Color Palette", order = 0)]
public class ColorPaletteSO : ScriptableObject
{
    public Color NumberNormal;
    public Color NumberPositive;
    public Color NumberNegative;
    public Color TextNormal { get; set; }
}
}