using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace FontUpdate;

class PluginInfo
{
    public const string GUID = "rectorado.FontUpdate";
    public const string Name = "FontUpdate";
    public const string Version = "0.8.2";
}

public enum FontMode
{
    Normal,
    NoDollar
}

public static class FontRegexPatterns
{
    public const string NormalRegexPattern = @"^(b|DialogueText|3270.*)$";
    public const string NoDollarRegexPattern = @"^(b|DialogueText|3270.*)$";
    public const string TransmitRegexPattern = @"^edunline.*$";
}

public static class FontPaths
{
    public const string FontAssetPath = @"FontUpdate";
}

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
class Plugin : BaseUnityPlugin
{
    // font-related configurations
    public static ConfigEntry<bool> configNormalIngameFont;
    public static ConfigEntry<bool> configTransmitIngameFont;
    public static ConfigEntry<FontMode> configFontMode;
    public static ConfigEntry<int> configChatCharacterLimit;

    // debug-related configurations
    public static ConfigEntry<bool> configDebugLog;
    public static Plugin Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        configFontMode = Config.Bind(
            "General",
            "FontMode",
            FontMode.NoDollar,
            "NoDollar: Updated font without dollar sign, like vanilla. Normal: Updated font with dollar sign."
        );
   configChatCharacterLimit = Config.Bind(
            "General",
            "Chat Character Limit",
            50,
            "Sets a character limit to the chat, set to -1 to have it unlimited."
        );

        configNormalIngameFont = Config.Bind(
            "General",
            "Use Default Font",
            false,
            "Uses the vanilla font, mainly for testing purposes"
        );

        configTransmitIngameFont = Config.Bind(
            "General",
            "UsingTransmitIngameFont",
            true,
            "Uses the vanilla font, don't deactivate this as it will break the font"
        );


configDebugLog = Config.Bind(
    "Debug",
    "Log",
    false,
    new ConfigDescription("")
);

        FontLoader.Load();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }

    public static void LogInfo(string msg)
    {
        if (!configDebugLog.Value) return;

        Instance.Logger.LogInfo(msg);
    }

    public static void LogWarning(string msg)
    {
        if (!configDebugLog.Value) return;

        Instance.Logger.LogWarning(msg);
    }

    public static void LogError(string msg)
    {
        Instance.Logger.LogError(msg);
    }
}
