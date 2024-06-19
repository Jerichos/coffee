namespace POLYGONWARE.Coffee.Settings
{
public class GameSettings
{
    public const uint BASE_COFFEES_TO_LEVEL = 50;
    public const float LEVEL_DIFFICULTY_MULTIPLIER = 2.5f;
}

public struct PlayerSettingsData
{
    public float MusicVolume;
    public float SfxVolume;
    public bool Vibration;
    public bool Notifications;
    public bool AutoSave;
}
}