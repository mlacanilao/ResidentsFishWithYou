using System;
using System.Runtime.CompilerServices;
using BepInEx;
using HarmonyLib;
using ResidentsFishWithYou.UI;

namespace ResidentsFishWithYou;

internal static class ModInfo
{
    internal const string Guid = "omegaplatinum.elin.residentsfishwithyou";
    internal const string Name = "Residents Fish With You";
    internal const string Version = "2.0.0";
    internal const string ModOptionsGuid = "evilmask.elinplugins.modoptions";
}

[BepInPlugin(GUID: ModInfo.Guid, Name: ModInfo.Name, Version: ModInfo.Version)]
internal class ResidentsFishWithYou : BaseUnityPlugin
{
    internal static ResidentsFishWithYou? Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        ResidentsFishWithYouConfig.LoadConfig(config: Config);
        Harmony.CreateAndPatchAll(type: typeof(Patcher), harmonyInstanceId: ModInfo.Guid);

        if (HasModOptionsPlugin() == false)
        {
            return;
        }

        try
        {
            UIController.RegisterUI();
        }
        catch (Exception ex)
        {
            LogError(message: $"An error occurred during UI registration: {ex}");
        }
    }

    internal static void LogDebug(object message, [CallerMemberName] string caller = "")
    {
        Instance?.Logger.LogDebug(data: $"[{caller}] {message}");
    }
    
    internal static void LogError(object message)
    {
        Instance?.Logger.LogError(data: message);
    }

    private static bool HasModOptionsPlugin()
    {
        try
        {
            foreach (var obj in ModManager.ListPluginObject)
            {
                if (obj is not BaseUnityPlugin plugin)
                {
                    continue;
                }

                if (plugin.Info.Metadata.GUID == ModInfo.ModOptionsGuid)
                {
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            LogError(message: $"Error while checking for Mod Options: {ex}");
            return false;
        }
    }
}
