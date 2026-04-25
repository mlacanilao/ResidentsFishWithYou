using System.IO;
using System.Reflection;
using BepInEx.Configuration;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;

namespace ResidentsFishWithYou.UI;

public static class UIController
{
    public static void RegisterUI()
    {
        ModOptionController controller = ModOptionController.Register(guid: ModInfo.Guid, tooptipId: "mod.tooltip");
        if (controller == null)
        {
            ResidentsFishWithYou.LogError(message: "Failed to register Mod Options controller.");
            return;
        }

        string assemblyLocation = Path.GetDirectoryName(path: Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        string xmlPath = Path.Combine(path1: assemblyLocation, path2: "ResidentsFishWithYouConfig.xml");
        string xlsxPath = Path.Combine(path1: assemblyLocation, path2: "translations.xlsx");

        ResidentsFishWithYouConfig.InitializeTranslationXlsxPath(xlsxPath: xlsxPath);

        if (File.Exists(path: xmlPath))
        {
            controller.SetPreBuildWithXml(xml: File.ReadAllText(path: xmlPath));
        }
        else
        {
            ResidentsFishWithYou.LogError(message: $"Mod Options XML not found: {xmlPath}");
        }

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
            BindConfigToggle(
                toggle: GetRequiredPreBuild<OptToggle>(
                    builder: builder,
                    id: "enableAutoPlaceFishingItemsToggle"),
                configEntry: ResidentsFishWithYouConfig.EnableAutoPlaceFishingItems);
            BindConfigToggle(
                toggle: GetRequiredPreBuild<OptToggle>(
                    builder: builder,
                    id: "enableRequireBaitToggle"),
                configEntry: ResidentsFishWithYouConfig.EnableRequireBait);
        };
    }

    private static void BindConfigToggle(OptToggle? toggle, ConfigEntry<bool> configEntry)
    {
        if (toggle == null)
        {
            return;
        }

        toggle.Checked = configEntry.Value;
        toggle.OnValueChanged += isChecked =>
        {
            configEntry.Value = isChecked;
        };
    }

    private static T? GetRequiredPreBuild<T>(OptionUIBuilder builder, string id) where T : OptUIElement
    {
        T? element = builder.GetPreBuild<T>(id: id);
        if (element == null)
        {
            ResidentsFishWithYou.LogError(message: $"Missing Mod Options prebuilt element: {id}");
        }

        return element;
    }
}
