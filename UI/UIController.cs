using System.IO;
using System.Reflection;
using BepInEx.Configuration;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;

namespace ResidentsFishWithYou.UI;

public static class UIController
{
    private const string GeneralLangId = "general";

    public static void RegisterUI()
    {
        ModOptionController controller = ModOptionController.Register(guid: ModInfo.Guid, tooptipId: "mod.tooltip");
        if (controller == null)
        {
            ResidentsFishWithYou.LogError(message: "Failed to register Mod Options controller.");
            return;
        }

        string assemblyLocation = Path.GetDirectoryName(path: Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        string xlsxPath = Path.Combine(path1: assemblyLocation, path2: "translations.xlsx");

        ResidentsFishWithYouConfig.InitializeTranslationXlsxPath(xlsxPath: xlsxPath);

        if (File.Exists(path: ResidentsFishWithYouConfig.TranslationXlsxPath))
        {
            controller.SetTranslationsFromXslx(path: ResidentsFishWithYouConfig.TranslationXlsxPath);
        }
        else
        {
            ResidentsFishWithYou.LogError(message: $"Mod Options translations not found: {xlsxPath}");
        }

        RegisterEvents(controller: controller);
    }

    private static void RegisterEvents(ModOptionController controller)
    {
        controller.OnBuildUI += builder =>
        {
            OptVLayout optionsGroup = builder.Root.AddVLayoutWithBorder(title: Lang.Get(GeneralLangId));
            AddConfigToggle(
                parent: optionsGroup,
                configEntry: ResidentsFishWithYouConfig.EnableAutoPlaceFishingItems);
            AddConfigToggle(
                parent: optionsGroup,
                configEntry: ResidentsFishWithYouConfig.EnableRequireBait);
        };
    }

    private static void AddConfigToggle(OptLayout parent, ConfigEntry<bool> configEntry)
    {
        OptToggle toggle = parent.AddToggle(
            text: configEntry.Definition.Key,
            isChecked: configEntry.Value,
            tooltip: configEntry.Description.Description);
        toggle.OnValueChanged += isChecked =>
        {
            configEntry.Value = isChecked;
        };
    }
}
