using BepInEx;
using HarmonyLib;

namespace ResidentsFishWithYou
{
    internal static class ModInfo
    {
        internal const string Guid = "omegaplatinum.elin.residentsfishwithyou";
        internal const string Name = "Residents Fish with You";
        internal const string Version = "1.2.1.0";
    }

    [BepInPlugin(GUID: ModInfo.Guid, Name: ModInfo.Name, Version: ModInfo.Version)]
    internal class ResidentsFishWithYou : BaseUnityPlugin
    {
        internal static ResidentsFishWithYou Instance { get; private set; }
        
        private void Start()
        {
            Instance = this;
            
            ResidentsFishWithYouConfig.LoadConfig(config: Config);

            Harmony.CreateAndPatchAll(type: typeof(Patcher), harmonyInstanceId: ModInfo.Guid);
        }
        
        internal static void Log(object payload)
        {
            Instance?.Logger.LogInfo(data: payload);
        }
    }
}