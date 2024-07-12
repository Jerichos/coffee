using UnityEngine;

namespace POLYGONWARE.Coffee.Utilities
{
public static class TextFormatter
{
    /// <summary>
    /// Colors a string for TextMeshPro and optionally makes it bold.
    /// </summary>
    /// <param name="text">The text to color.</param>
    /// <param name="color">The color to apply.</param>
    /// <param name="bold">Whether to make the text bold.</param>
    /// <returns>The colored (and optionally bold) text as a string.</returns>
    public static string ColorText(string text, Color color, bool bold = false)
    {
        string hexColor = ColorToHex(color);
        string colorTag = $"<color=#{hexColor}>";
        string endColorTag = "</color>";
        string boldTag = bold ? "<b>" : "";
        string endBoldTag = bold ? "</b>" : "";

        return $"{colorTag}{boldTag}{text}{endBoldTag}{endColorTag}";
    }

    /// <summary>
    /// Converts a Color to a hexadecimal string.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>The hexadecimal string representation of the color.</returns>
    private static string ColorToHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGBA(color);
        // int r = Mathf.RoundToInt(color.r * 255f);
        // int g = Mathf.RoundToInt(color.g * 255f);
        // int b = Mathf.RoundToInt(color.b * 255f);
        // int a = Mathf.RoundToInt(color.a * 255f);
        // return $"{r:X2}{g:X2}{b:X2}{a:X2}";
    }
}
}