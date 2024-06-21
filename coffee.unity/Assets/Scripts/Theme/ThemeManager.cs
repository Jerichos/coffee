using UnityEngine;

namespace POLYGONWARE.Coffee.Theme
{
public class ThemeManager : MonoBehaviour
{
    [SerializeField] ColorPaletteSO _colorPaletteSO;
    
    public static ThemeManager Instance { get; private set; }
    
    public static ColorPaletteSO ColorPalette => Instance._colorPaletteSO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }
}
}